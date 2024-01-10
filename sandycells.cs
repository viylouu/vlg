using SimulationFramework;

partial class sandycells {

    public static sandy.cell sand = new sandy.cell() {
        col = new Color(247, 208, 118),
        name = "sand",

        move = true,
        liquid = false
    };

    public static sandy.cell stone = new sandy.cell() {
        col = new Color(84, 62, 84),
        name = "stone",

        move = false,
        liquid = false
    };

    public static sandy.cell water = new sandy.cell() {
        col = new Color(69, 106, 161),
        name = "water",

        move = true,
        liquid = true
    };
}