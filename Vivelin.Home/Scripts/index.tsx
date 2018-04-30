interface TwitchFollowsProps {
    userId: string
    accessToken: string
}

interface TwitchFollowsState {
    user: Twitch.User
    streams: Twitch.Stream[]
    streamsUsers: Twitch.User[]
    errorName: string
    errorMessage: string
}

class TwitchStream extends React.Component<{ stream: Twitch.Stream, user: Twitch.User }> {
    render() {
        if (!this.props.user) {
            return null
        }

        const streamUrl = 'https://www.twitch.tv/' + this.props.user.login
        const thumbnailUrl = this.props.stream.thumbnail_url.replace('{width}', '640').replace('{height}', '360')
        return (
            <div className='twitchStream' style={{backgroundImage: 'url(' + thumbnailUrl + ')'}}>
                <header>
                    <a href={streamUrl} target='blank' rel='external' className='twitchStream-profileLink'>
                        <img src={this.props.user.profile_image_url} alt={this.props.user.display_name} title={this.props.user.display_name} className='twitchStream-profileImage' />
                    </a>
                    <b className='twitchStream-title'>{this.props.stream.title}</b>
                </header>
            </div>
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
            errorName: null,
            errorMessage: null
        }
    }

    componentDidMount() {
        this.fetchUser()
        this.fetchStreams()
    }

    render() {
        const isMissingData = (!this.state.user || !this.state.streams || !this.state.streamsUsers)
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
            return <TwitchStream key={index} user={user} stream={stream} />
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