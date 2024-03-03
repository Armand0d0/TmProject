package game;



import graphic.*;
import org.joml.Vector3f;
import org.joml.Vector4f;
import engine.IHud;
import engine.Window;

import java.awt.*;

public class Hud implements IHud {


    private static final Font FONT = new Font("", Font.BOLD, 20);

    private static final String CHARSET = "ISO-8859-1";

    private final GameItem[] gameItems;

    private Color fontColor = new Color(1,1,1,1);

    private Color backGroundColor = new Color(1,1,1,0);

    private final FontTexture fontTexture = new FontTexture(FONT, CHARSET ,fontColor ,backGroundColor);


    public Hud( int i ) throws Exception {
        gameItems = new GameItem[i];
//                                                                            String fonts[] =
//                                                                                    GraphicsEnvironment.getLocalGraphicsEnvironment().getAvailableFontFamilyNames();
//
//                                                                            for ( int l = 0; l < fonts.length; l++ )
//                                                                            {
//                                                                                System.out.println(fonts[l]);
//                                                                            }
    }



    @Override
    public GameItem[] getGameItems() {
        return gameItems;
    }

    public void updateSize(int i ,Window window) {
        gameItems[i].setPosition(10f, window.getHeight() - 50f, 0);
    }

    public void setImage(int i , Texture t){
        float[] normals = new float[0];
        int[] idx  = new int[]{0,1,2,2,0,3};
        float[] texCoords = new float[]{1,1,      0,1,     0,0,     1,0};
        float[] pos = new float[]{t.getWidth(),t.getHeight(),0,    t.getWidth(),-t.getHeight(),0,    -t.getWidth(),-t.getHeight(),0,     -t.getWidth(),t.getHeight(),0};
               Mesh mesh = new Mesh(pos,idx,texCoords,normals);
        Material m = new Material(t,0);
        mesh.setMaterial(m);
        gameItems[i] = new GameItem(mesh);
    }


    public void  setText(int i ,String s) throws Exception{
        gameItems[i]=new TextItem(s ,fontTexture);
    }
    public void setFontColor(Vector4f c){
        this.fontColor = new Color(c.x,c.y,c.z,c.w);
    }
    public void setFontColor(float r ,float g,float b,float a){
        this.fontColor = new Color(r,g,b,a);
    }
    public void setBackGroundColor(Vector4f c){
        this.backGroundColor = new Color(c.x,c.y,c.z,c.w);
    }
    public void setBackGroundColor(float r ,float g,float b,float a){
        this.backGroundColor = new Color(r,g,b,a);
    }

}

