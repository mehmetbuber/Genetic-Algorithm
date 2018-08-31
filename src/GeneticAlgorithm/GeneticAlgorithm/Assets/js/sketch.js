var graphData = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 20, 19];

function setup() {
    background(30, 43, 60);
    // Sets the screen to be 720 pixels wide and 400 pixels high
    var width = document.getElementById('graph').offsetWidth;
    var cnv = createCanvas(width, 300);
    //cnv.style('display', 'block');
    cnv.parent("graph");
    noSmooth();
    noLoop();
    textAlign(CENTER);

}

function draw() {

    // put drawing code here
}


function drawGraph() {
    background(30, 43, 60);
    stroke(255);
    noFill();
    beginShape();

    var prevData = 0;
    var prevX = 0;
    for (var i = 0; i < graphData.length; i++) {
        stroke(255);
        var y = height - map(graphData[i], 0, variableCount, 0, height - 20);
        var x = map(i, 0, graphData.length, 0, width - 40) + 10;
        vertex(x, y);

        fill(255);
        if (prevData !== graphData[i] && x - prevX > 40 && i != graphData.length - 1) {
            ellipse(x, y, 5, 5);
            textSize(12);
            text(graphData[i], x + 5, y - 5);
            text(i, x, height - 5);
            prevX = x;
        }
        if (i === graphData.length - 1) {
            textSize(16);
            text(graphData[i], x + 5, y - 5);
            text(i, x + 5, height - 5);
        }
        noFill();

        prevData = graphData[i];
    }

    endShape();

    beginShape();
    vertex(10, height);
    vertex(width - 10, height);
    endShape();
}