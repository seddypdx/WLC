

$(document).ready(function () {

    $('#MemberLookup').change(function () {
        this.form.submit();
    });
   
    $('.member-checkbox').change(function () {
        var theName = $(this).val();
        var existing = $('#Checkin_Note').val();

        if ($(this).is(":checked")) {
            if (existing === '')
                existing = theName;
            else
                existing = existing + ', ' + theName;
        }
        else {
            existing = existing.replace(theName + ', ', '');
            existing = existing.replace(theName, '');
        }

        $('#Checkin_Note').val(existing);
    });

});

