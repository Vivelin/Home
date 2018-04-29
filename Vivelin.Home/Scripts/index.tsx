interface TwitchFollowsProps {
    userId: string
    accessToken: string
}

class TwitchStream extends React.Component<{ stream: Twitch.Stream, user: Twitch.User }> {
    render() {
        const streamUrl = 'https://www.twitch.tv/' + this.props.user.login
        return <a href={streamUrl} target='blank' rel='external'>{this.props.user.display_name} streaming {this.props.stream.title}</a>
    }
}

class TwitchFollows extends React.Component<TwitchFollowsProps, { user: Twitch.User, streams: Twitch.Stream[], streamsUsers: Twitch.User[], errorName: string, errorMessage: string }> {
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
        const isMissingData = (!this.state.user || !this.state.streams || !this.state.streamsUsers);
        if (isMissingData && !this.state.errorMessage) {
            return <p>Loading data…</p>
        }
        else if (isMissingData) {
            if (this.state.errorName === 'Unauthorized') {
                return null
            }

            return <p>{this.state.errorName}: {this.state.errorMessage}</p>
        }

        return (
            <ul>
                {
                    this.state.streams.map((stream, index) => {
                        const user = this.state.streamsUsers.filter(x => x.id === stream.user_id)[0]
                        return <li key={index}><TwitchStream user={user} stream={stream} /></li>
                    })
                }
            </ul>)
    }

    private fetchUser() {
        this.sendRequest<Twitch.User>('users?id=' + this.props.userId, response => {
            this.setState({
                user: response.data[0]
            })
        })
    }

    private fetchStreams() {
        this.sendRequest<Twitch.Follow>('users/follows?first=100&from_id=' + this.props.userId, response => {
            const streamsUri = 'streams?user_id=' + response.data.map(x => x.to_id).join('&user_id=')
            this.sendRequest<Twitch.Stream>(streamsUri, response => {
                this.setState({
                    streams: response.data
                })
            })

            const usersUri = 'users?id=' + response.data.map(x => x.to_id).join('&id=')
            this.sendRequest<Twitch.User>(usersUri, response => {
                this.setState({
                    streamsUsers: response.data
                });
            })
        })
    }

    private sendRequest<T>(relativeUri: string, successCallback: (data: Twitch.Response<T>) => void) {
        var request = new XMLHttpRequest()
        request.onload = e => {
            const response = JSON.parse(request.responseText) as Twitch.Response<T>;
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
    const container = document.getElementById('twitch-follows');
    if (container) {
        const userId = container.dataset.userId
        const accessToken = container.dataset.accessToken
        ReactDOM.render(<TwitchFollows userId={userId} accessToken={accessToken} />, container)
    }
}