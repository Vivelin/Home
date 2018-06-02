/**
 * Represents a loading indicator that can be toggled on or off. 
 */
class LoadingIndicator extends React.Component<{ visible: boolean }> {
    render() {
        return <div className='loader' hidden={!this.props.visible} ></div>
    }
}