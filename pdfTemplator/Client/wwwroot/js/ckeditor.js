CKEditorInterop = (() => {
    var editors = {};

    return {
        init(id, dotNetReference) {
            window.DecoupledEditor
                .create(document.getElementById(id))
                .then(editor => {
                    editors[id] = editor;

                    var oldValue = document.getElementById(id).getAttribute("value");
                    if (oldValue.length > 0) editor.setData(oldValue);

                    const toolbarContainer = document.querySelector('#toolBar-'+id);
                    toolbarContainer.appendChild(editor.ui.view.toolbar.element);

                    editor.model.document.on('change:data', () => {
                        var data = editor.getData();

                        //var el = document.createElement('div');
                        //el.innerHTML = data;
                        //if (el.innerText.trim() === '')
                        //    data = null;

                        dotNetReference.invokeMethodAsync('EditorDataChanged', data);
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