window.tinyMceConfig = {
    height: 500,
    plugins: 'preview searchreplace autolink autosave directionality code visualblocks visualchars fullscreen image link media template codesample table charmap pagebreak nonbreaking anchor insertdatetime advlist lists wordcount help charmap quickbars emoticons',
    toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview | insertfile image media template link anchor codesample | ltr rtl',
    toolbar_sticky: true,
}

function insertIntoEditor(text) {
    tinymce.activeEditor.execCommand('mceInsertContent', false, text);
}