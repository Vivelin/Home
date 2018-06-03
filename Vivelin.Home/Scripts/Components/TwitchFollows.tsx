/**
 * Contains properties for the TwitchFollows class.
 */
interface TwitchFollowsProps {
    /**
     * The user ID of the Twitch user whose followed channels to display.
     */
    userId: string

    /**
     * A Twitch OAuth access token used to access the Twitch API.
     */
    accessToken: string

    /**
     * The number of seconds between each reload of the Twitch API data.
     */
    reloadInterval: number
}

/**
 * Contains the state for the TwitchFollows class.
 */
interface TwitchFollowsState {
    /**
     * The number of API requests that have not completed yet.
     */
    pendingRequests: number

    /**
     * The ID of the interval timer that reloads the Twitch API data.
     */
    reloadIntervalId: number

    /**
     * The logged-in Twitch user whose followed channels are displayed.
     */
    user: Twitch.User

    /**
     * A collection of the streams to display.
     */
    streams: Twitch.Stream[]

    /**
     * A collection of the followed users who are currently streaming.
     */
    streamsUsers: Twitch.User[]

    /**
     * The name of the Twitch API error, if any.
     */
    errorName: string

    /**
     * The Twitch API error message, if any.
     */
    errorMessage: string
}

/**
 * Shows the live Twitch channels followed by the logged-in user.
 */
class TwitchFollows extends React.Component<TwitchFollowsProps, TwitchFollowsState> {
    /**
     * Initializes a new instance of the TwitchFollows component with the specified props.
     * @param props The properties to initialize the class with.
     */
    constructor(props: TwitchFollowsProps) {
        super(props)
        this.state = {
            pendingRequests: 0,
            reloadIntervalId: null,

            user: null,
            streams: null,
            streamsUsers: null,
            errorName: null,
            errorMessage: null
        }
    }

    /**
     * Occurs after the component has been placed in the DOM.
     */
    componentDidMount() {
        if (!this.props.userId || !this.props.accessToken)
            return

        this.fetchUser()
        this.fetchStreams()

        const id = window.setInterval(() => this.fetchStreams(), this.props.reloadInterval)
        this.setState({ reloadIntervalId: id })
    }

    /**
     * Occurs before the component will be removed from the DOM.
     */
    componentWillUnmount() {
        window.clearInterval(this.state.reloadIntervalId)
    }

    /**
     * Updates the component.
     */
    render() {
        const isMissingData = (!this.state.user || !this.state.streams || !this.state.streamsUsers)

        if (this.state.errorName) {
            return <p>{this.state.errorName}: {this.state.errorMessage}</p>
        }

        if (isMissingData) {
            return <LoadingIndicator visible={this.state.pendingRequests > 0} />
        }

        if (this.state.pendingRequests === 0 && this.state.streams.length == 0) {
            return <RandomElement>
                <p>Nobody you’re following on Twitch is currently live.</p>
                <p className='with image'>Maybe there’s something on <a href='https://www.youtube.com/feed/subscriptions/activity' rel='external'>YouTube</a>? <img src='https://static-cdn.jtvnw.net/emoticons/v1/904875/3.0' alt='rooShrug' /></p>
                <p className='with image'><img src='https://static-cdn.jtvnw.net/emoticons/v1/904819/3.0' alt='rooBlank' /></p>
            </RandomElement>
        }

        return (
            <>
                <LoadingIndicator visible={this.state.pendingRequests > 0} />
                {
                    this.state.streams
                        .sort(TwitchStream.SortByStartDateDescending)
                        .filter(TwitchStream.IsLive)
                        .map((stream, index) => {
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
                streams: this.merge(prevState.streams, response.data, x => x.id)
            }))
        })

        const usersUri = 'users?id=' + response.data.map(x => x.to_id).join('&id=')
        this.sendRequest<Twitch.User>(usersUri, response => {
            this.setState((prevState) => ({
                streamsUsers: this.merge(prevState.streamsUsers, response.data, x => x.id)
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

    private merge<T>(oldValues: T[], newValues: T[], key: (value: T) => any): T[] {
        if (!oldValues) {
            return newValues
        }

        return oldValues
            .filter(oldValue => !newValues.some(newValue => key(newValue) === key(oldValue)))
            .concat(newValues)
    }
}

{
    const container = document.getElementById('twitch-follows')
    if (container) {
        const userId = container.dataset.userId
        const accessToken = container.dataset.accessToken
        ReactDOM.render(<TwitchFollows reloadInterval={30000} userId={userId} accessToken={accessToken} />, container)
    }
}