using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour {

    public int width = 18;
    public int height = 18;
    public Vector3 offset;

    public GameObject wall;

    private float haltonSequence1D(int b, int index){
        float res = 0;
        int denom = 1;

        if( index <= 0 )
            return 0f;

        while( index > 0 ){
            denom *= b;
            res += (float)(index % b) / denom;
            index = (int)( (float)index / b );
        }

        return res;
    }

    // Use this for initialization
    void Start () {
        // GenerateLevel();
    }
    
    // Create a level using Random.Range()
    public void GenerateRandomLevel()
    {
        float coverage = GameObject.Find("CoverageSlider").GetComponent<Slider>().value;

        // Find total number of objects to generate
        int total = (int)((width+1) * (height+1) * coverage);

        // Place <total> objects using Unity's built-in Random() function
        for(int i=0; i<total; i++){
            float randX = Random.Range(0f, (float)width);
            float randY = Random.Range(0f, (float)height);
            Vector3 pos = new Vector3(offset.x + randX - width / 2f, offset.y + 1f, offset.z + randY - height / 2f);
            Instantiate(wall, pos, Quaternion.identity, transform);
        }

    }

    // Create a level using a Halton sequence
    public void GenerateHaltonLevel()
    {
        float coverage = GameObject.Find("CoverageSlider").GetComponent<Slider>().value;

        // Coprime bases for a 2D Halton sequence
        int baseX = 2;
        int baseY = 3;

        // For variability, generate a random offset for the Halton sequence
        int hOffset = Random.Range(0, 500);

        // Find total number of objects to generate
        int total = (int)((width+1) * (height+1) * coverage);

        // Place <total> objects using a Halton sequence
        for(int i=0; i<total; i++){
            float randX = width * haltonSequence1D(baseX, i + hOffset);
            float randY = height * haltonSequence1D(baseY, i + hOffset);
            Vector3 pos = new Vector3(offset.x + randX - width / 2f, offset.y + 1f, offset.z + randY - height / 2f);
            Instantiate(wall, pos, Quaternion.identity, transform);
        }
    }

    class Disk{
        public float x = 0;
        public float y = 0;
        public float radius = 1f;

        public Disk(float xx, float yy, float rr){
            x = xx;
            y = yy;
            radius = rr;
        }
    }

    private bool checkDiskFree( List<Disk> placed, Disk d ){
        foreach( Disk pd in placed ){
            Vector3 dist = new Vector3(pd.x - d.x, 0, pd.y - d.y);
            if( dist.magnitude < 2*d.radius )
                return false;
        }

        return true;
    }

    // Create a level using a Poisson distribution
    public void GeneratePoissonLevel()
    {
        int MAX_TRIES = 6;
        float separation = 1.0f;
        float radius = 0.5f;
        float coverage = GameObject.Find("CoverageSlider").GetComponent<Slider>().value;
        // Find total number of objects to generate
        int total = (int)((width+1) * (height+1) * coverage);

        Disk initial = new Disk(0, 0, radius);
        Vector3 pos = new Vector3(0,1,0);
        Instantiate(wall, pos, Quaternion.identity, transform);

        List<Disk> active = new List<Disk>();
        List<Disk> placed = new List<Disk>();

        active.Add( initial );
        placed.Add( initial );

        while( active.Count > 0 && placed.Count < total ){
            Disk current = active[0];
            bool cFlag = false;

            for(int i=0; i<MAX_TRIES; i++){
                float angle = (float)i / MAX_TRIES * 2 * Mathf.PI;
                float r = 2 * radius + separation * Random.value;
                Disk d = new Disk(current.x + r * Mathf.Cos( angle ),
                                  current.y + r * Mathf.Sin( angle ),
                                  radius);

                if( checkDiskFree(placed, d) ){
                    pos = new Vector3(offset.x + d.x, offset.y + 1f, offset.z + d.y);
                    Instantiate(wall, pos, Quaternion.identity, transform);

                    placed.Add( d );
                    active.Add( d );
                    cFlag = true;
                    break;
                }
            }
            if( !cFlag )
                active.RemoveAt(0);
        }        

        /*
        // For variability, generate a random offset for the Halton sequence
        int hOffset = Random.Range(0, 500);

        // Find total number of objects to generate
        int total = (int)((width+1) * (height+1) * coverage);

        // Place <total> objects using a Halton sequence
        for(int i=0; i<total; i++){
            float randX = width * haltonSequence1D(baseX, i + hOffset);
            float randY = height * haltonSequence1D(baseY, i + hOffset);
            Vector3 pos = new Vector3(offset.x + randX - width / 2f, offset.y + 1f, offset.z + randY - height / 2f);
            Instantiate(wall, pos, Quaternion.identity, transform);
        }
        */
    }

    // Create a level using <>
    public void GenerateLevel()
    {
        float coverage = GameObject.Find("CoverageSlider").GetComponent<Slider>().value;

        // Loop over the grid
        for (int x = 0; x <= width; x+=1)
        {
            for (int y = 0; y <= height; y+=1)
            {
                // Should we place a wall?
                if (Random.value > 1-coverage)
                {
                    // Spawn a wall
                    Vector3 pos = new Vector3(offset.x + x - width / 2f, offset.y + 1f, offset.z + y - height / 2f);
                    Instantiate(wall, pos, Quaternion.identity, transform);
                }
            }
        }
    }
     

    // Clear spawned objects
    public void ClearLevel(){
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach( GameObject go in obstacles )
            Destroy( go );
    }

}