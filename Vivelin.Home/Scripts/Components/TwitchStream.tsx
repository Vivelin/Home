/**
 * Represents a live Twitch stream.
 */
class TwitchStream extends React.Component<{ stream: Twitch.Stream, user: Twitch.User }> {
    /**
     * When used as a comparison function, streams that have more viewers will be sorted before other streams.
     * @param a A Twitch stream to sort.
     * @param b Another Twitch stream to sort.
     */
    public static SortByViewersDescending(a: Twitch.Stream, b: Twitch.Stream): number {
        return b.viewer_count - a.viewer_count
    }

    /**
     * When used as a comparison function, streams that just started will before sorted before streams that have been live for longer.
     * @param a A Twitch stream to sort.
     * @param b Another Twitch stream to sort.
     */
    public static SortByStartDateDescending(a: Twitch.Stream, b: Twitch.Stream): number {
        return Date.parse(b.started_at) - Date.parse(a.started_at)
    }

    /**
     * Returns a value indicating whether the specified stream is currently live.
     * @param value A Twitch stream.
     */
    public static IsLive(value: Twitch.Stream): boolean {
        return value.type != 'vodcast'
    }

    /**
     * Updates the component.
     */
    public render() {
        if (!this.props.user) {
            return null
        }

        const streamUrl = 'https://www.twitch.tv/' + this.props.user.login
        const thumbnailUrl = this.props.stream.thumbnail_url.replace('{width}', '640').replace('{height}', '360') + '#' + Date.now()

        return (
            <figure className='twitchStream'>
                <a href={streamUrl} rel='external'>
                    <img src={thumbnailUrl} alt={this.props.stream.title} className='twitchStream-thumbnail' />
                </a>

                <figcaption className='overlay'>
                    <a href={streamUrl} rel='external'>
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
            return this.uptime() + ' · no viewers'
        if (viewerCount === 1)
            return this.uptime() + ' · 1 viewer'
        return this.uptime() + ' · ' + viewerCount.toLocaleString() + ' viewers';
    }

    private uptime() {
        const seconds = 1000
        const minutes = 60 * seconds
        const hours = 60 * minutes
        const days = 24 * hours // Probably overkill

        const startedAt = new Date(this.props.stream.started_at)
        const elapsedMs = Date.now() - startedAt.getTime()

        if (elapsedMs < 15 * minutes) {
            return 'Going live'
        }
        else if (elapsedMs < 1.5 * hours) {
            const elapsedMinutes = elapsedMs / minutes
            return Math.round(elapsedMinutes) + ' minutes'
        }
        else if (elapsedMs < 1.5 * days) {
            const elapsedHours = elapsedMs / hours
            return Math.round(elapsedHours) + ' hours'
        }
        else {
            const elapsedDays = elapsedMs / days;
            return Math.round(elapsedDays) + ' days'
        }
    }
}