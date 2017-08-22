
$.extend($.messager.defaults, {
    ok: "确定",
    cancel: "取消"
});

$.extend($.fn.validatebox.defaults.rules, {
    equals: {
        validator: function (value, param) {
            return value == $(param[0]).val();
        },
        message: '两次输入的密码不相同'
    }
});

$.extend($.fn.datagrid.defaults, {
    //height: 510,
    fit: true,
    pageSize: 15,
    pageList: [10, 15, 20, 30],
    singleSelect: true,
    striped: true,
    border: false,
    rownumbers: true,
    pagination: true,
    sortName: 'ID',
    sortOrder: 'asc',
    loadMsg: '正在为您加载数据，请稍候……',
    onSelect: function (rowIndex, rowData) {
        var tb = $(this).datagrid('options').toolbar;
        $(tb).find('.easyui-linkbutton[select]').linkbutton('enable');
    },
    onLoadSuccess: function (data) {
        var tb = $(this).datagrid('options').toolbar;
        $(tb).find('.easyui-linkbutton[select]').linkbutton('disable');
        return true;
    },
    resize: function () {
        height: 100;
    }
});

$.extend($.fn.pagination.defaults, {
    pageList: [10, 15, 20, 30],
    beforePageText: '第',
    afterPageText: '页，共 {pages} 页',
    displayMsg: '显示第 {from} - {to} 条，共 {total} 条'
});

$.extend($.fn.validatebox.defaults, {
    //missingMessage: '必填项'
});

$.extend($.fn.datebox.defaults, {
    formatter: function (date) {
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        return y + '/' + (m < 10 ? ('0' + m) : m) + '/' + (d < 10 ? ('0' + d) : d);
    }
});

$.extend($.fn.panel.defaults, {
    
});

function showSuccess(msg) {
    $.messager.show({     
        title: '',
        msg: '<div class="icon-msgok" style="padding-left:25px;color:Green;">' + msg + '</div>',
        height: 'auto',
        width: 'auto',
        style: {
            right: '',
            top: document.body.scrollTop + document.documentElement.scrollTop + 30,
            bottom: '',
            'min-width': '250px'
        }
    });
    return true;
}

function showError(msg) {
    $.messager.show({
        title: '',
        msg: '<div class="icon-msgerr" style="padding-left:25px;color:Red;">' + msg + '</div>',
        height: 'auto',
        width: 'auto',
        timeout: 5000,
        style: {
            right: '',
            top: document.body.scrollTop + document.documentElement.scrollTop + 30,
            bottom: '',
            'min-width': '250px'
        }
    });

    return false;
}

function destroyRow(dg,url) {
    var row = $(dg).datagrid('getSelected');
    if (row) {
        var index = $(dg).datagrid('getRowIndex', row);
        $.messager.confirm('删除', '是否删除当前记录?', function (r) {
            if (r) {
                $.ajax({
                    url: url,
                    data: { id: row.ID, rspType: 'Script' },
                    type: 'POST',
                    success: function (data) {
                        if (data.success) {
                            showSuccess(data.message);
                            $(dg).datagrid('deleteRow',index);
                        }
                        else {
                            showError(data.message);
                        }
                    },
                    error: function (data, t) {
                        alert("error:[" + t + "]" + data.responseText);
                    }
                });
            }
        });
    }
}

function getFormJson(frm) {
    var o = {};
    var a = $(frm).serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });

    return o;
}

function doSearch(fm,dg) {
    var data = getFormJson(fm);
    $(dg).datagrid({
        queryParams: data,
        pageNumber: 1
    });
    $(dg).datagrid('reload');
}

function redirectUrl(url) {
    window.location.href = url;
}