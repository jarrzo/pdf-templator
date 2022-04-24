CKEditorInterop = (() => {
    var editors = {};

    return {
        init(id, dotNetReference) {
            window.DecoupledDocumentEditor
                .create(document.getElementById(id))
                .then(editor => {
                    editors[id] = editor;

                    var oldValue = document.getElementById(id).getAttribute("value");
                    if (oldValue != null) editor.setData(oldValue);

                    const toolbarContainer = document.querySelector('#toolBar-'+id);
                    toolbarContainer.appendChild(editor.ui.view.toolbar.element);

                    editor.model.document.on('change:data', () => {
                        dotNetReference.invokeMethodAsync('EditorDataChanged', editor.getData());
                    });
                })
                .catch(error => console.error(error));
        },
        destroy(id) {
            editors[id].destroy()
                .then(() => delete editors[id])
                .catch(error => console.log(error));
        }
    };
})();