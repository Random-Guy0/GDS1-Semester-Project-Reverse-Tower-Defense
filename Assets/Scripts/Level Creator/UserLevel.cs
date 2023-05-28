using System;

[Serializable]
public class UserLevel
{
    public int levelWidth;
    public int levelDepth;

    public int[] grid;
    public float[] heights;

    public int[] towers;
    public int[] towerPositionsX;
    public int[] towerPositionsZ;
    public float[] towerSpawnTimes;

    public int pathPieces;
    public int gateHealth;
    public int startingMana;
    public int maxMana;
    
    public UserLevel(int levelWidth, int levelDepth, int[] grid, float[] heights, int[] towers, int[] towerPositionsX, int[] towerPositionsZ, float[] towerSpawnTimes, int pathPieces, int gateHealth, int startingMana, int maxMana)
    {
        this.levelWidth = levelWidth;
        this.levelDepth = levelDepth;
        this.grid = grid;
        this.heights = heights;
        this.towers = towers;
        this.towerPositionsX = towerPositionsX;
        this.towerPositionsZ = towerPositionsZ;
        this.towerSpawnTimes = towerSpawnTimes;
        this.pathPieces = pathPieces;
        this.gateHealth = gateHealth;
        this.startingMana = startingMana;
        this.maxMana = maxMana;
    }
}