// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


<script>

var fullPathsMap = new Map();
var thumbPathsMap = new Map();


    @foreach (ForumThread thread in Model.Threads) {
        @foreach (Post post in thread) {
            if (post.HasImage) {

                <text>
                    fullPathsMap.set('@post.Files[0].FullFileName', '@post.Files[0].FullFileName')
                    thumbPathsMap.set('@post.Files[0].FullFileName', '@post.Files[0].ThumbFileName')
                </text>

            }
        }
    }

</script>

<script>

    function resizeImage(event) { 

        var image = event.target;
        

        if (image.getAttribute("data-expanded") === "0") {
            image.src = '/images/' + fullPathsMap.get(image.id);
            image.dataset.expanded = "1";
        } else {
            image.src = '/images/' + thumbPathsMap.get(image.id);
            image.dataset.expanded = "0";
        }


    }
</script>