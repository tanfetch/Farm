

$(document).ready(function () {

    $("input.auto").each(function () {
        var self = this;
        var key = $(self).attr("key");
        var multiple = $(self).attr("multiple") == null ? false : true;
        var minChars = $(self).attr("minChars") == null ? 1 : $(self).attr("minChars");
        var max = $(self).attr("max") == null ? 10 : $(self).attr("max");
        $.get("/Helper/AutoComplete", { id: key }, function (data) {
            $(self).autocomplete(data, { minChars: minChars, multiple: multiple, max: max });
        });
    });

});

//异步提交表单
function do_ajaxSumit(obj) {
    var t = $(obj).attr("ajaxTarget");
    $(obj).ajaxSubmit(function (data) {
        $(t).html(data);
    });
};

//排序
function order_inc(obj, property) {
    $("#o_1").val(property);
    $("#o_0").val("");
    //$("#p").val(1);
    do_ajaxSumit($(obj).parents("form"));
};
function order_desc(obj, property) {
    $("#o_0").val(property);
    $("#o_1").val("");
    //$("#p").val(1);
    do_ajaxSumit($(obj).parents("form"));
};

function do_search(obj) {
    $("#p").val(1);
    do_ajaxSumit($(obj).parents("form"));
};

function go_pager(obj, p) {
    //alert($(obj).parents("form").attr("ajaxTarget"));
    $("#p").val(p);
    do_ajaxSumit($(obj).parents("form"));
};

