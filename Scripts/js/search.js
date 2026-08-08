var common = {
    init: function () {
        common.registerEvent();
    },

    registerEvent: function () {
        $("#txtkeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/ListName",
                    datatype: "json",
                    data: {
                        q: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },

            focus: function (event, ui) {
                $("#txtkeyword").val(ui.item.label);
                return false;
            },

            select: function (event, ui) {
                $("#txtkeyword").val(ui.item.label);
                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div>" + item.label + "</div>")
                    .appendTo(ul);
            };
    }
}
common.init();