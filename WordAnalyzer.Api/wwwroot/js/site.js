$(function()
{
    var $textArea = $('#textArea');
    var $output = $('#output')

    $('#loadOrder').on('click', function()
    {

        if ($textArea.val().length > 0)
        {
            var order = {
                text : $textArea.val()
            };

            $.ajax({
                url: '/converts/load',
                dataType: 'json',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(order),
                processData: false,
                statusCode: {
                    201 : function(newOrder) {
                        $('#buttons').css('display', 'block');
                    },
                    204 : function() {
                        alert('Error, please insert text');
                    }
                },
                error: function()
                {
                    $.ajaxSetup({
                        error: function (x, status, error) {
                            alert("An error occurred: " + status + "nError: " + error);
                        }
                    });
                }
            });
        };
    })

    $('#toXml').on('click', function()
    {
        $.ajax({
            type: 'GET',
            url: '/converts/ConvertToXml',
            success: function(output)
            {
                console.log('output', output);
                $output.html(output);
            },
            error: function()
            {
                alert('error during conversion to xml');
            }
        });
    })

    $('#toCsv').on('click', function()
    {
        $.ajax({
            type: 'GET',
            url: '/converts/ConvertToCsv',
            success: function(output)
            {
                $output.html(output);
            },
            error: function()
            {
                alert('error during conversion to csv');
            }
        });
    })
})