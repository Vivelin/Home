{
    function reloadViewComponents(): void {
        const viewComponents = $('[data-src]')
        viewComponents.each(function () {
            const $viewComponent = $(this)
            const srcAddress = $viewComponent.data('src')
            $.get(srcAddress, function (data: string) {
                $viewComponent.html(data)
            })
        })
    }

    $(function () {
        reloadViewComponents()
        window.setInterval(reloadViewComponents, 5000)
    })
}