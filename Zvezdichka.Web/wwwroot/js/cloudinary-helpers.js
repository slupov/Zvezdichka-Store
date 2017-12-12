function addMultipleUpload(elementId, folder, itemName) {
    document.getElementById(elementId).addEventListener("click",
        function() {
            cloudinary.openUploadWidget(
                {
                    cloud_name: 'zvezdichka',
                    upload_preset: 'unsigned-1',
                    multiple: true,
                    folder: folder,
                    public_id: itemName,
                    max_file_size: 5000000,
                    theme: 'white'
                },
                function(error, result) { console.log(error, result) });
        },
        false);
}