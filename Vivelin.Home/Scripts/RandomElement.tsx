/**
 * Selects a single element from any of the component’s children at random.
 */
class RandomElement extends React.Component {
    render() {
        if (this.props.children instanceof Array) {
            var children = this.props.children as any[]
            var theChosenOne = Math.floor(Math.random() * children.length)
            return children[theChosenOne]
        }

        return this.props.children;
    }
}