function ShowMessage(message, messagetype) {
    var cssclass;
    var icon;
    var leyenda;
    switch (messagetype) {
        case 'Plain':
            cssclass = 'alert-default'
            icon = ''
            leyenda=''
            break;
        case 'Success':
            cssclass = 'alert-success'
            icon = 'check'
            leyenda = '<b>Confirmación!</b> '
            break;
        case 'Information':
            cssclass = 'alert-info'
            icon = 'info-circle'
            leyenda = '<b>Notificación!</b> '
            break;
        case 'Warning':
            cssclass = 'alert-warning'
            icon = 'warning'
            leyenda = '<b>Alerta!</b> '
            break;
        case 'Danger':
            cssclass = 'alert-danger'
            icon = 'remove'
            leyenda = '<b>Error!</b> '
            break;
        case 'Primary':
            cssclass = 'alert-primary'
            icon = ''
            leyenda = ''
            break;
        default:
            cssclass = 'alert-info'
            icon = ''
            leyenda = ''
    }
    /*$('#alert_container').append('<div id="alert_div" style="margin: 0 10%; -webkit-box-shadow: 3px 4px 6px #999;width:40%" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');*/
    $('#alert_container').append('<div id="alert_div" style="margin: 0 10%; -webkit-box-shadow: 3px 4px 6px #999;width:40%" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong><i class="fa fa-' + icon +'"></i></strong> <span>' + leyenda+message + '</span></div>');
    return false;
}