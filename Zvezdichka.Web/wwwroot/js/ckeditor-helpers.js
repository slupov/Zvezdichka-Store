function addCkEditor(textareaId, formId) {
    //attaches the editor to the text area
    CKEDITOR.replace(textareaId);

    updateEditorElements();
    if (formId === "") {
        return;
    }

    $("#" + formId).on("submit", updateEditorElements);
}

function updateEditorElements() {
    var instance;
    for (instance in CKEDITOR.instances) {
        if (CKEDITOR.instances.hasOwnProperty(instance)) {
            CKEDITOR.instances[instance].updateElement(() => all);
        }
    }
}