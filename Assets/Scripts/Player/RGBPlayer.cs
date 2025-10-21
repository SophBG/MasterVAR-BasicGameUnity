using UnityEngine;

public class RGBPlayer : MonoBehaviour
{
    private float hue = 0;
    public float speed;
    public Renderer player;
    private bool isRGBEnabled = false;

    // Update is called once per frame
    public void Update()
    {
        if (isRGBEnabled)
        {
            hue += Time.deltaTime * speed * 0.1f;
            if (hue > 1f)
                hue -= 1f;

            player.material.color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }

    public void ToggleRGB(bool rgb)
    {
        isRGBEnabled = rgb;
        if (!isRGBEnabled)
            player.material.color = new Color(37/255, 37/255, 37/255);
    }
}
