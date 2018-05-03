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
var TwitchStream = (function (_super) {
    __extends(TwitchStream, _super);
    function TwitchStream() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TwitchStream.prototype.render = function () {
        if (!this.props.user || !this.props.game) {
            return null;
        }
        var streamUrl = 'https://www.twitch.tv/' + this.props.user.login;
        var thumbnailUrl = this.props.stream.thumbnail_url.replace('{width}', '640').replace('{height}', '360');
        return (React.createElement("figure", { className: 'twitchStream' },
            React.createElement("a", { href: streamUrl, target: '_blank', rel: 'external' },
                React.createElement("img", { src: thumbnailUrl, alt: this.props.stream.title, className: 'twitchStream-thumbnail' })),
            React.createElement("figcaption", null,
                React.createElement("a", { href: streamUrl, target: '_blank', rel: 'external' },
                    React.createElement("img", { src: this.props.user.profile_image_url, alt: this.props.user.display_name, title: this.props.user.display_name, className: 'twitchStream-profileImage' }),
                    React.createElement("b", { className: 'twitchStream-title', title: this.props.stream.title }, this.props.stream.title),
                    React.createElement("span", { className: 'twitchStream-description' },
                        this.props.user.display_name,
                        " // ",
                        this.props.game.name)))));
    };
    return TwitchStream;
}(React.Component));
var TwitchFollows = (function (_super) {
    __extends(TwitchFollows, _super);
    function TwitchFollows(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            user: null,
            streams: null,
            streamsUsers: null,
            games: null,
            errorName: null,
            errorMessage: null
        };
        return _this;
    }
    TwitchFollows.prototype.componentDidMount = function () {
        this.fetchUser();
        this.fetchStreams();
    };
    TwitchFollows.prototype.render = function () {
        var _this = this;
        var isMissingData = (!this.state.user || !this.state.streams || !this.state.streamsUsers || !this.state.games);
        if (isMissingData && !this.state.errorMessage) {
            return React.createElement("p", null, "Loading data\u2026");
        }
        else if (isMissingData) {
            if (this.state.errorName === 'Unauthorized') {
                return null;
            }
            return React.createElement("p", null,
                this.state.errorName,
                ": ",
                this.state.errorMessage);
        }
        return this.state.streams.map(function (stream, index) {
            var user = _this.state.streamsUsers.filter(function (x) { return x.id === stream.user_id; })[0];
            var game = _this.state.games.filter(function (x) { return x.id === stream.game_id; })[0];
            return React.createElement(TwitchStream, { key: index, user: user, stream: stream, game: game });
        });
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
                streams: prevState.streams ? prevState.streams.concat(response.data) : response.data
            }); });
            var gamesUri = 'games?id=' + response.data.map(function (x) { return x.game_id; }).join('&id=');
            _this.sendRequest(gamesUri, function (response) {
                _this.setState(function (prevState) { return ({
                    games: prevState.games ? prevState.games.concat(response.data) : response.data
                }); });
            });
        });
        var usersUri = 'users?id=' + response.data.map(function (x) { return x.to_id; }).join('&id=');
        this.sendRequest(usersUri, function (response) {
            _this.setState(function (prevState) { return ({
                streamsUsers: prevState.streamsUsers ? prevState.streamsUsers.concat(response.data) : response.data
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
        request.send();
    };
    return TwitchFollows;
}(React.Component));
{
    var container = document.getElementById('twitch-follows');
    if (container) {
        var userId = container.dataset.userId;
        var accessToken = container.dataset.accessToken;
        ReactDOM.render(React.createElement(TwitchFollows, { userId: userId, accessToken: accessToken }), container);
    }
}
//# sourceMappingURL=index.js.map