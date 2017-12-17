function addCkEditor(toReplace, formId) {
        CKEDITOR.replace(toReplace);

        if (formId === "") {
            return;
        }

        $("#" + formId).on("submit",
            function() {
                var instance;
                for (instance in CKEDITOR.instances) {
                    if (CKEDITOR.instances.hasOwnProperty(instance)) {
                        CKEDITOR.instances[instance].updateElement(() => all);
                    }
                }
            });
}