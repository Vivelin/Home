interface TwitchFollowsProps {
    userId: string
    accessToken: string
}

interface TwitchFollowsState {
    pendingRequests: number
    user: Twitch.User
    streams: Twitch.Stream[]
    streamsUsers: Twitch.User[]
    errorName: string
    errorMessage: string
}

class LoadingIndicator extends React.Component<{ visible: boolean }> {
    render() {
        return <div className='loader' hidden={!this.props.visible} ></div>
    }
}

class TwitchStream extends React.Component<{ stream: Twitch.Stream, user: Twitch.User }> {
    render() {
        if (!this.props.user) {
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
                        <span className='twitchStream-description'>{this.description()}</span>
                    </a>
                </figcaption>
            </figure>
        )
    }

    private description() {
        const viewerCount = this.props.stream.viewer_count
        if (viewerCount === 0)
            return this.uptime() + ', all alone'
        if (viewerCount === 1)
            return this.uptime() + ' for a lone soul'
        return this.uptime() + ' for ' + viewerCount.toLocaleString() + ' viewers';
    }

    private uptime() {
        const seconds = 1000
        const minutes = 60 * seconds
        const hours = 60 * minutes
        const days = 24 * hours // Probably overkill

        const startedAt = new Date(this.props.stream.started_at)
        const elapsedMs = Date.now() - startedAt.getTime()

        if (elapsedMs < 10 * minutes) {
            return '🆕'
        }
        else if (elapsedMs < 1.5 * hours) {
            const elapsedMinutes = elapsedMs / minutes
            return Math.round(elapsedMinutes) + 'm'
        }
        else if (elapsedMs < 1.5 * days) {
            const elapsedHours = elapsedMs / hours
            return Math.round(elapsedHours) + 'h'
        }
        else {
            const elapsedDays = elapsedMs / days;
            return '~' + Math.round(elapsedDays) + 'd'
        }
    }
}

class TwitchFollows extends React.Component<TwitchFollowsProps, TwitchFollowsState> {
    constructor(props: TwitchFollowsProps) {
        super(props)
        this.state = {
            pendingRequests: 0,
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

        if (isMissingData && this.state.errorName) {
            if (this.state.errorName === 'Unauthorized') {
                return null
            }

            return <p>{this.state.errorName}: {this.state.errorMessage}</p>
        }

        return (
            <>
            <LoadingIndicator visible={this.state.pendingRequests > 0} />
            {
                !isMissingData && this.state.streams.filter(x => x.type != 'vodcast').map((stream, index) => {
                    const user = this.state.streamsUsers.filter(x => x.id === stream.user_id)[0]
                    return <TwitchStream key={index} user={user} stream={stream} />
                })
            }
            </>
        )
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
            this.setState((prevState) => ({ pendingRequests: prevState.pendingRequests - 1 }))

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

        this.setState((prevState) => ({ pendingRequests: prevState.pendingRequests + 1 }));
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