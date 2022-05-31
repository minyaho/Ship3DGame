using UnityEngine;


public class BulletHole : MonoBehaviour
{
    private Renderer m_Renderer;
    private RaycastHit hit;
    public Texture2D bolt;
    void Awake()
    {
        //獲取到渲染元件
        m_Renderer = GetComponent<Renderer>();
    }
    void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {

            Texture2D texture = (Texture2D)m_Renderer.material.mainTexture;
            Vector2 point = hit.textureCoord;
            point.x *= texture.width;
            point.y *= texture.height;

            //texture.SetPixels((int)point.x, (int)point.y, bolt.width, bolt.height, bolt.GetPixels());

            for (int i = 0; i < bolt.width; i++)
            {
                for (int j = 0; j < bolt.height; j++)
                {
                    Color c = bolt.GetPixel(i, j);
                    if (c.a >= 0.2f)
                    {
                        texture.SetPixel((int)point.x + i, (int)point.y + j, c);
                    }
                }
            }
            texture.Apply();
        }
    }

}