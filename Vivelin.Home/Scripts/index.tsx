interface TwitchFollowsProps {
    userId: string
    accessToken: string
}

interface TwitchFollowsState {
    user: Twitch.User
    streams: Twitch.Stream[]
    streamsUsers: Twitch.User[]
    games: Twitch.Game[]
    errorName: string
    errorMessage: string
}

class TwitchStream extends React.Component<{ stream: Twitch.Stream, user: Twitch.User, game: Twitch.Game }> {
    render() {
        if (!this.props.user || !this.props.game) {
            return null
        }

        const streamUrl = 'https://www.twitch.tv/' + this.props.user.login
        const thumbnailUrl = this.props.stream.thumbnail_url.replace('{width}', '640').replace('{height}', '360')
        return (
            <figure className='twitchStream'>
                <a href={streamUrl} target='_blank' rel='external'>
                    <img src={thumbnailUrl} alt={this.props.stream.title} className='twitchStream-thumbnail' />
                </a>

                <figcaption>
                    <a href={streamUrl} target='_blank' rel='external'>
                        <img src={this.props.user.profile_image_url} alt={this.props.user.display_name} title={this.props.user.display_name} className='twitchStream-profileImage' />
                        <b className='twitchStream-title' title={this.props.stream.title}>{this.props.stream.title}</b>
                        <span className='twitchStream-description'>{this.props.user.display_name} // {this.props.game.name}</span>
                    </a>
                </figcaption>
            </figure>
        )
    }
}

class TwitchFollows extends React.Component<TwitchFollowsProps, TwitchFollowsState> {
    constructor(props: TwitchFollowsProps) {
        super(props)
        this.state = {
            user: null,
            streams: null,
            streamsUsers: null,
            games: null,
            errorName: null,
            errorMessage: null
        }
    }

    componentDidMount() {
        this.fetchUser()
        this.fetchStreams()
    }

    render() {
        const isMissingData = (!this.state.user || !this.state.streams || !this.state.streamsUsers || !this.state.games)
        if (isMissingData && !this.state.errorMessage) {
            return <p>Loading data…</p>
        }
        else if (isMissingData) {
            if (this.state.errorName === 'Unauthorized') {
                return null
            }

            return <p>{this.state.errorName}: {this.state.errorMessage}</p>
        }

        return this.state.streams.map((stream, index) => {
            const user = this.state.streamsUsers.filter(x => x.id === stream.user_id)[0]
            const game = this.state.games.filter(x => x.id === stream.game_id)[0]
            return <TwitchStream key={index} user={user} stream={stream} game={game} />
        })
    }

    private fetchUser() {
        this.sendRequest<Twitch.User>('users?id=' + this.props.userId, response => {
            this.setState({
                user: response.data[0]
            })
        })
    }

    private fetchStreams() {
        this.sendRequest<Twitch.Follow>('users/follows?from_id=' + this.props.userId, response => this.loadStreams(response))
    }

    private loadStreams(response: Twitch.Response<Twitch.Follow>) {
        if (response.data.length == 0) {
            return
        }

        const streamsUri = 'streams?user_id=' + response.data.map(x => x.to_id).join('&user_id=')
        this.sendRequest<Twitch.Stream>(streamsUri, response => {
            this.setState((prevState) => ({
                streams: prevState.streams ? prevState.streams.concat(response.data) : response.data
            }))

            const gamesUri = 'games?id=' + response.data.map(x => x.game_id).join('&id=')
            this.sendRequest<Twitch.Game>(gamesUri, response => {
                this.setState((prevState) => ({
                    games: prevState.games ? prevState.games.concat(response.data) : response.data
                }))
            })
        })

        const usersUri = 'users?id=' + response.data.map(x => x.to_id).join('&id=')
        this.sendRequest<Twitch.User>(usersUri, response => {
            this.setState((prevState) => ({
                streamsUsers: prevState.streamsUsers ? prevState.streamsUsers.concat(response.data) : response.data
            }))
        })

        if (response.pagination) {
            this.sendRequest<Twitch.Follow>('users/follows?from_id=' + this.props.userId + '&after=' + response.pagination.cursor, response => this.loadStreams(response))
        }
    }

    private sendRequest<T>(relativeUri: string, successCallback: (data: Twitch.Response<T>) => void) {
        var request = new XMLHttpRequest()
        request.onload = e => {
            const response = JSON.parse(request.responseText) as Twitch.Response<T>
            if (response.error) {
                this.setState({
                    errorName: response.error,
                    errorMessage: response.message
                })
            } else {
                successCallback(response)
            }
        }

        request.open('GET', 'https://api.twitch.tv/helix/' + relativeUri)
        request.setRequestHeader('Authorization', 'Bearer ' + this.props.accessToken)
        request.send()
    }
}

{
    const container = document.getElementById('twitch-follows')
    if (container) {
        const userId = container.dataset.userId
        const accessToken = container.dataset.accessToken
        ReactDOM.render(<TwitchFollows userId={userId} accessToken={accessToken} />, container)
    }
}