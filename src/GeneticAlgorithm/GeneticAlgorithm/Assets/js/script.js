var done = false;
var interval;
var valueCount = 0;
var variableCount = 0;

$(document).ready(function () {
    valueCount = parseInt($('#value-count').val());
    variableCount = parseInt($('#variable-count').val());
});

$('#value-count').change(function () {
    valueCount = parseInt($('#value-count').val());
});

function evolveAjax() {
    if (!done) {
        setTimeout(function () {
            var postData = {
                evolutionId: $('#evolutionId').val()
            };
            $.ajax({
                url: '/Home/Evolve',
                dataType: 'json',
                data: JSON.stringify(postData),
                type: 'post',
                contentType: 'application/json',
                success: function (data) {
                    colorMembersJson(data.Population, data.Target);
                    $('#generation').html(data.Generation);
                    $('#totalMembers').html(data.TotalMembers);
                    graphData = data.History;
                    drawGraph();
                    evolveAjax();
                    if (data.Done)
                        done = true;
                },

                error: function (jqXhr, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        },
             1000 - parseInt($('#evolutionSpeed').val()));
    }
}

function evolveAjaxFast() {
    $.ajax({
        url: '/Home/EvolveToEnd',
        dataType: 'json',
        data: JSON.stringify({ evolutionId: $('#evolutionId').val() }),
        type: 'post',
        contentType: 'application/json',
        success: function (data) {
            $('#generation').html("&nbsp " + data.Generation);
            $('#totalMembers').html("&nbsp " + data.TotalMembers);
            colorMembersJson(data.Population, data.Target);
            graphData = data.History;
            drawGraph();
        },

        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

function colorMembersJson(population, targetMember) {
    var target = document.getElementById('target');
    var variables = targetMember.Variables;

    colorLine(target, variables);

    for (var i = 0; i < population.length; i++) {
        var memberID = 'member' + i.toString();
        var member = document.getElementById(memberID);
        member.childNodes.item(0).innerText = population[i].Fitness;
        colorLine(member, population[i].Variables);
    }
}

function colorLine(doc, variables) {

    for (var i = 0; i < variables.length; i++) {

        $(doc.childNodes.item(i + 1)).css("background-color", getColor(variables[i]));
        $(doc.childNodes.item(i + 1)).html(variables[i]);
    }
}

function getColor(variable) {
    return perc2color(variable);
}

function startEvolution() {
    evolveAjax();
}

function startEvolutionFast() {
    evolveAjaxFast();
}

function perc2color(variable) {
    var perc = (variable + 1) * (100 / valueCount);
    var b;
    var r = 50;
    var g = 50;
    if (perc < 50) {
        b = Math.round(4 * perc);
    }
    else {
        b = Math.round(400 - 4 * perc);
    }
    var h = r * 0x10000 + g * 0x100 + b * 0x1;
    return '#' + ('000000' + h.toString(16)).slice(-6);
}

function createButton() {
    var variableCount = parseInt(document.getElementById('variable-count').value);
    var valueCount = parseInt(document.getElementById('value-count').value);
    var population = parseInt(document.getElementById('population-count').value);
    var link = window.location.origin +
        "/" +
        population.toString() +
        "/" +
        valueCount.toString() +
        "/" +
        variableCount.toString();
    window.location.href = link;
}

function spawnMembers() {
    //Clear members
    var myNode = document.getElementById("members-panel");
    while (myNode.firstChild) {
        myNode.removeChild(myNode.firstChild);
    }

    var variableCount = parseInt(document.getElementById('variable-count').value);
    var valueCount = parseInt(document.getElementById('value-count').value);
    var population = parseInt(document.getElementById('population-count').value);

    createTarget(variableCount);
    createPopulation(variableCount, population);
}

//Create Target
function createTarget(variableCount) {
    var target = document.createElement('div')
    target.className = 'member-div';
    target.id = 'target';
    document.getElementById('members-panel').appendChild(target);

    var targetLabel = document.createElement('label');
    targetLabel.innerHTML = "T";
    targetLabel.className = "fitness-label";
    target.appendChild(targetLabel);

    for (var i = 0; i < variableCount; i++) {
        var square = document.createElement('div');
        square.className = 'square';
        document.getElementById('target').appendChild(square);
    }
}

//Create Population
function createPopulation(variableCount, population) {
    for (var j = 0; j < population; j++) {
        var div = document.createElement('div');
        div.className = 'member-div';
        div.id = 'member' + j.toString();
        document.getElementById('members-panel').appendChild(div);
        var label = document.createElement('label');
        label.innerHTML = "0";
        label.className = "fitness-label";
        div.appendChild(label);
        for (var i = 0; i < variableCount; i++) {
            var square = document.createElement('div');
            square.className = 'square';
            document.getElementById('member' + j).appendChild(square);
        }
    }
}

