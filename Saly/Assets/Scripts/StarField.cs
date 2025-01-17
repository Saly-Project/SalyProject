using UnityEngine;
using UnityEngine.Rendering;

public class StarField : MonoBehaviour
{


    private Transform thisTransform;
    public ParticleSystem particleSystemStars;
    private ParticleSystem.Particle[] points;

    public int starMax = 100;
    public float starSize = 1f;
    public float starDistance = 10f;
    public float starClipDistance = 1f;

    private float starDistanceSquare;
    private float starClipDistanceSquare;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisTransform = transform;
        starClipDistanceSquare = starClipDistance * starClipDistance;
        starDistanceSquare = starDistance * starDistance;

    }
    
    private void CreateStars(){
        points =  new ParticleSystem.Particle[starMax];
        
        // setting parameters for each stars of the array
        for (int i = 0; i < starMax; ++i)
        {
            points[i].position = Random.insideUnitSphere * starDistance + thisTransform.position;
            points[i].startColor = new Color(1, 1, 1, 1);
            points[i].startSize = starSize;
            //EmitStar(Random.insideUnitSphere * starDistance + thisTransform.position, 1f, starSize);
        }

    }


    

    // Update is called once per frame
    void Update()
    {

        if (points == null){
            CreateStars();
        }

        for (int i = 0; i < starMax; ++i){
            if ((points[i].position - thisTransform.position).sqrMagnitude > starDistanceSquare){
                points[i].position = Random.insideUnitSphere.normalized * starDistance + thisTransform.position;
            }
            if ((points[i].position - thisTransform.position).sqrMagnitude <= starClipDistanceSquare){
                float percent = (points[i].position - thisTransform.position).sqrMagnitude / starClipDistanceSquare;
                points[i].startColor = new Color(1, 1, 1, percent); //reduce alpha opacity based on the cam position
                points[i].startSize = percent * starSize;
            }

            particleSystemStars.SetParticles(points, points.Length);
        }
    }
}
