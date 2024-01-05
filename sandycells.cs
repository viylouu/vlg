using SimulationFramework;

partial class sandycells {

    public static sandy.cell sand = new sandy.cell() {
        col = new Color(237, 188, 90, 255),
        name = "sand",

        move = true,
        liquid = false
    };

    public static sandy.cell stone = new sandy.cell() {
        col = new Color(136, 132, 145),
        name = "stone",

        move = false,
        liquid = false
    };

    public static sandy.cell water = new sandy.cell() {
        col = Color.Blue,
        name = "water",

        move = true,
        liquid = true
    };
}