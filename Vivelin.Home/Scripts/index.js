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
        var streamUrl = 'https://www.twitch.tv/' + this.props.user.login;
        return React.createElement("a", { href: streamUrl, target: 'blank', rel: 'external' },
            this.props.user.display_name,
            " streaming ",
            this.props.stream.title);
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
        var isMissingData = (!this.state.user || !this.state.streams || !this.state.streamsUsers);
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
        return (React.createElement("ul", null, this.state.streams.map(function (stream, index) {
            var user = _this.state.streamsUsers.filter(function (x) { return x.id === stream.user_id; })[0];
            return React.createElement("li", { key: index },
                React.createElement(TwitchStream, { user: user, stream: stream }));
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
        this.sendRequest('users/follows?first=100&from_id=' + this.props.userId, function (response) {
            var streamsUri = 'streams?user_id=' + response.data.map(function (x) { return x.to_id; }).join('&user_id=');
            _this.sendRequest(streamsUri, function (response) {
                _this.setState({
                    streams: response.data
                });
            });
            var usersUri = 'users?id=' + response.data.map(function (x) { return x.to_id; }).join('&id=');
            _this.sendRequest(usersUri, function (response) {
                _this.setState({
                    streamsUsers: response.data
                });
            });
        });
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