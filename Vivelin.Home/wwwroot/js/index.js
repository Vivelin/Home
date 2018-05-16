var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var LoadingIndicator = (function (_super) {
    __extends(LoadingIndicator, _super);
    function LoadingIndicator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    LoadingIndicator.prototype.render = function () {
        return React.createElement("div", { className: 'loader', hidden: !this.props.visible });
    };
    return LoadingIndicator;
}(React.Component));
var TwitchStream = (function (_super) {
    __extends(TwitchStream, _super);
    function TwitchStream() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TwitchStream.SortByViewersDescending = function (a, b) {
        return b.viewer_count - a.viewer_count;
    };
    TwitchStream.IsLive = function (value) {
        return value.type != 'vodcast';
    };
    TwitchStream.prototype.render = function () {
        if (!this.props.user) {
            return null;
        }
        var streamUrl = 'https://www.twitch.tv/' + this.props.user.login;
        var thumbnailUrl = this.props.stream.thumbnail_url.replace('{width}', '640').replace('{height}', '360') + '#' + Date.now();
        return (React.createElement("figure", { className: 'twitchStream' },
            React.createElement("a", { href: streamUrl, target: '_blank', rel: 'external' },
                React.createElement("img", { src: thumbnailUrl, alt: this.props.stream.title, className: 'twitchStream-thumbnail' })),
            React.createElement("figcaption", { className: 'overlay' },
                React.createElement("a", { href: streamUrl, target: '_blank', rel: 'external' },
                    React.createElement("img", { src: this.props.user.profile_image_url, alt: this.props.user.display_name, title: this.props.user.display_name, className: 'twitchStream-profileImage' }),
                    React.createElement("b", { className: 'twitchStream-title', title: this.props.stream.title }, this.props.stream.title),
                    React.createElement("span", { className: 'twitchStream-description' }, this.description())))));
    };
    TwitchStream.prototype.description = function () {
        var viewerCount = this.props.stream.viewer_count;
        if (viewerCount === 0)
            return this.uptime() + ' alone';
        if (viewerCount === 1)
            return this.uptime() + ' for a lone soul';
        return this.uptime() + ' with ' + viewerCount.toLocaleString() + ' viewers';
    };
    TwitchStream.prototype.uptime = function () {
        var seconds = 1000;
        var minutes = 60 * seconds;
        var hours = 60 * minutes;
        var days = 24 * hours;
        var startedAt = new Date(this.props.stream.started_at);
        var elapsedMs = Date.now() - startedAt.getTime();
        if (elapsedMs < 15 * minutes) {
            return 'Going live';
        }
        else if (elapsedMs < 1.5 * hours) {
            var elapsedMinutes = elapsedMs / minutes;
            return Math.round(elapsedMinutes) + 'm';
        }
        else if (elapsedMs < 1.5 * days) {
            var elapsedHours = elapsedMs / hours;
            return Math.round(elapsedHours) + 'h';
        }
        else {
            var elapsedDays = elapsedMs / days;
            return '~' + Math.round(elapsedDays) + 'd';
        }
    };
    return TwitchStream;
}(React.Component));
var TwitchFollows = (function (_super) {
    __extends(TwitchFollows, _super);
    function TwitchFollows(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            pendingRequests: 0,
            reloadIntervalId: null,
            user: null,
            streams: null,
            streamsUsers: null,
            errorName: null,
            errorMessage: null
        };
        return _this;
    }
    TwitchFollows.prototype.componentDidMount = function () {
        var _this = this;
        if (!this.props.userId || !this.props.accessToken)
            return;
        this.fetchUser();
        this.fetchStreams();
        var id = window.setInterval(function () { return _this.fetchStreams(); }, this.props.reloadInterval);
        this.setState({ reloadIntervalId: id });
    };
    TwitchFollows.prototype.componentWillUnmount = function () {
        window.clearInterval(this.state.reloadIntervalId);
    };
    TwitchFollows.prototype.render = function () {
        var _this = this;
        var isMissingData = (!this.state.user || !this.state.streams || !this.state.streamsUsers);
        if (isMissingData && this.state.errorName) {
            if (this.state.errorName === 'Unauthorized') {
                return null;
            }
            return React.createElement("p", null,
                this.state.errorName,
                ": ",
                this.state.errorMessage);
        }
        return (React.createElement(React.Fragment, null,
            React.createElement(LoadingIndicator, { visible: this.state.pendingRequests > 0 }),
            !isMissingData && this.state.streams
                .sort(TwitchStream.SortByViewersDescending)
                .filter(TwitchStream.IsLive)
                .map(function (stream, index) {
                var user = _this.state.streamsUsers.filter(function (x) { return x.id === stream.user_id; })[0];
                return React.createElement(TwitchStream, { key: index, user: user, stream: stream });
            })));
    };
    TwitchFollows.prototype.fetchUser = function () {
        var _this = this;
        this.sendRequest('users?id=' + this.props.userId, function (response) {
            _this.setState({
                user: response.data[0]
            });
        });
    };
    TwitchFollows.prototype.fetchStreams = function () {
        var _this = this;
        this.sendRequest('users/follows?from_id=' + this.props.userId, function (response) { return _this.loadStreams(response); });
    };
    TwitchFollows.prototype.loadStreams = function (response) {
        var _this = this;
        if (response.data.length == 0) {
            return;
        }
        var streamsUri = 'streams?user_id=' + response.data.map(function (x) { return x.to_id; }).join('&user_id=');
        this.sendRequest(streamsUri, function (response) {
            _this.setState(function (prevState) { return ({
                streams: _this.merge(prevState.streams, response.data, function (x) { return x.id; })
            }); });
        });
        var usersUri = 'users?id=' + response.data.map(function (x) { return x.to_id; }).join('&id=');
        this.sendRequest(usersUri, function (response) {
            _this.setState(function (prevState) { return ({
                streamsUsers: _this.merge(prevState.streamsUsers, response.data, function (x) { return x.id; })
            }); });
        });
        if (response.pagination) {
            this.sendRequest('users/follows?from_id=' + this.props.userId + '&after=' + response.pagination.cursor, function (response) { return _this.loadStreams(response); });
        }
    };
    TwitchFollows.prototype.sendRequest = function (relativeUri, successCallback) {
        var _this = this;
        var request = new XMLHttpRequest();
        request.onload = function (e) {
            _this.setState(function (prevState) { return ({ pendingRequests: prevState.pendingRequests - 1 }); });
            var response = JSON.parse(request.responseText);
            if (response.error) {
                _this.setState({
                    errorName: response.error,
                    errorMessage: response.message
                });
            }
            else {
                successCallback(response);
            }
        };
        request.open('GET', 'https://api.twitch.tv/helix/' + relativeUri);
        request.setRequestHeader('Authorization', 'Bearer ' + this.props.accessToken);
        this.setState(function (prevState) { return ({ pendingRequests: prevState.pendingRequests + 1 }); });
        request.send();
    };
    TwitchFollows.prototype.merge = function (oldValues, newValues, key) {
        if (!oldValues) {
            return newValues;
        }
        return oldValues
            .filter(function (oldValue) { return !newValues.some(function (newValue) { return key(newValue) === key(oldValue); }); })
            .concat(newValues);
    };
    return TwitchFollows;
}(React.Component));
{
    var container = document.getElementById('twitch-follows');
    if (container) {
        var userId = container.dataset.userId;
        var accessToken = container.dataset.accessToken;
        ReactDOM.render(React.createElement(TwitchFollows, { reloadInterval: 30000, userId: userId, accessToken: accessToken }), container);
    }
}
//# sourceMappingURL=index.js.map