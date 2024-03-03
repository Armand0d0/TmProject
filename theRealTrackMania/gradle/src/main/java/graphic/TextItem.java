package graphic;

import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.List;
import engine.Utils;

public class TextItem extends GameItem {

    private static final float ZPOS = 0.0f;

    private static final int VERTICES_PER_QUAD = 4;

    private String text;

//    private final int numCols;

//    private final int numRows;
    private final FontTexture fontTexture;

    public TextItem(String text, FontTexture fontTexture) throws Exception {
        super();
        this.text = text;
//       this. numCols = numCols;
//        this.numRows = numRows;
        this.fontTexture = fontTexture;
        setMesh(buildMesh());
//        Texture texture = new Texture(fontFileName);
//        this.setMesh(buildMesh(texture,numCols,numRows));

    }

    private Mesh buildMesh() throws Exception {

//        byte[] chars = text.getBytes(Charset.forName("ISO-8859-1"));
//        int numChars = chars.length;

        List<Float> positions = new ArrayList();
        List<Float> textCoords = new ArrayList();
        float[] normals   = new float[0];
        List<Integer> indices   = new ArrayList();
        char[] characters = text.toCharArray();
        int numChars = characters.length;

        float startx = 0;


//        float tileWidth = (float)texture.getWidth() / (float)numCols;
//        float tileHeight = (float)texture.getHeight() / (float)numRows;

                            for(int i=0; i<numChars; i++) {
                                FontTexture.CharInfo charInfo = fontTexture.getCharInfo(characters[i]);
                                positions.add(startx); // x
                                positions.add(0.0f); //y
                                positions.add(ZPOS); //z
                                textCoords.add( (float)charInfo.getStartX() / (float)fontTexture.getWidth());
                                textCoords.add(0.0f);
                                indices.add(i*VERTICES_PER_QUAD);

                                // Left Bottom vertex
                                positions.add(startx); // x
                                positions.add((float)fontTexture.getHeight()); //y
                                positions.add(ZPOS); //z
                                textCoords.add((float)charInfo.getStartX() / (float)fontTexture.getWidth());
                                textCoords.add(1.0f);
                                indices.add(i*VERTICES_PER_QUAD + 1);

                                // Right Bottom vertex
                                positions.add(startx + charInfo.getWidth()); // x
                                positions.add((float)fontTexture.getHeight()); //y
                                positions.add(ZPOS); //z
                                textCoords.add((float)(charInfo.getStartX() + charInfo.getWidth() )/ (float)fontTexture.getWidth());
                                textCoords.add(1.0f);
                                indices.add(i*VERTICES_PER_QUAD + 2);

                                // Right Top vertex
                                positions.add(startx + charInfo.getWidth()); // x
                                positions.add(0.0f); //y
                                positions.add(ZPOS); //z
                                textCoords.add((float)(charInfo.getStartX() + charInfo.getWidth() )/ (float)fontTexture.getWidth());
                                textCoords.add(0.0f);
                                indices.add(i*VERTICES_PER_QUAD + 3);

                                // Add indices por left top and bottom right vertices
                                indices.add(i*VERTICES_PER_QUAD);
                                indices.add(i*VERTICES_PER_QUAD + 2);

                                startx += charInfo.getWidth();


    }
        float[] posArr = Utils.listToArray(positions);
        float[] textCoordsArr = Utils.listToArray(textCoords);
        int[] indicesArr = indices.stream().mapToInt(i->i).toArray();
        Mesh mesh = new Mesh(posArr,indicesArr ,textCoordsArr ,normals );
        mesh.setMaterial(new Material(fontTexture.getTexture(),0));
        return mesh;
    }
    public String getText() {
        return text;
    }

    public void setText(String text) throws Exception {
        this.text = text;
        Texture texture = this.getMesh().getMaterial().getTexture();
        this.getMesh().deleteBuffers();
        this.setMesh(buildMesh());
    }

}
