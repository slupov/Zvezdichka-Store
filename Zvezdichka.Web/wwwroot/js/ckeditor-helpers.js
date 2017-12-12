function addCkEditor(toReplace, formId) {
    $(document).ready(function() {
        CKEDITOR.replace(toReplace);

        $("#" + formId).on("submit",
            function() {
                var instance;
                for (instance in CKEDITOR.instances) {
                    if (CKEDITOR.instances.hasOwnProperty(instance)) {
                        CKEDITOR.instances[instance].updateElement(() => all);
                    }
                }
            });
    });
}