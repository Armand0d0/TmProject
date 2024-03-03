package engine;

import graphic.Mesh;
import org.joml.Vector2f;
import org.joml.Vector3f;

import java.util.ArrayList;
import java.util.List;


public class X3DLoader {
	
	public X3DLoader() {
	}

	public static Mesh loadMesh(String fileName) throws Exception {
	List<String> lines = Utils.readAllLines(fileName);
	List<Vector3f> vertices = new ArrayList<>();
	List<Vector2f> textures = new ArrayList<>();
	List<Vector3f> normals = new ArrayList<>();
	
	List<TriangleFace> Trfaces = new ArrayList<>();
	
	
	
	String Transform = "Transform";
	
	
	String[] CoordDef = new String[] {
			/**/"Cube_TRANSFORM","Cube_ifs_TRANSFORM","group_ME_Cube","","","coords_ME_Cube"
			};
	String[] TextureDef = new String[] {
			/**/"Cube_TRANSFORM","Cube_ifs_TRANSFORM","group_ME_Cube","","",""
			};
	String[] IdxDef = new String[] {
			/**/"Cube_TRANSFORM","Cube_ifs_TRANSFORM","group_ME_Cube","",""
			};
	
	
	XMLDecoder decoder = new XMLDecoder();
		String Pos = decoder.decode(fileName, Transform, CoordDef, "point");
		String Text = decoder.decode(fileName, Transform,TextureDef,"point");
		vertices = toVec3fArray(Pos);
		textures = toVec2fArray(Text);
		System.out.println(vertices.toString());
		System.out.println(textures.toString());

		
		String CoordIdx = decoder.decode(fileName, Transform,IdxDef,"coordIndex");
		String TextIdx = decoder.decode(fileName, Transform,IdxDef,"texCoordIndex");
		System.out.println(CoordIdx);
		System.out.println(TextIdx);
	
	return reorderLists(vertices, textures, normals, Trfaces);
	}
	public static List<Vector3f> toVec3fArray(String s){
		List<Vector3f> list = new ArrayList<>();
		if(s != "") {
			
			
        String[] tokens = s.split("\\s+");
      for(int i=0;i<tokens.length;i+=3) {
    	  Vector3f v = new Vector3f(
               Float.parseFloat(tokens[i]),
               Float.parseFloat(tokens[i+1]),
               Float.parseFloat(tokens[i+2]));
    	  list.add(v);
      }
		}
        
    	   
       
      
	 return list;
 }
	public static List<Vector2f> toVec2fArray(String s){
        List<Vector2f> list = new ArrayList<>();
        if(s!="") {
        	String[] tokens = s.split("\\s+");
      for(int i=0;i<tokens.length;i+=2) {
    	  Vector2f v = new Vector2f(
               Float.parseFloat(tokens[i]),
               Float.parseFloat(tokens[i+1]));
               
    	  list.add(v);
      }
        }
        
    	   
       
      
	 return list;
 }
	
	private static Mesh reorderLists(List<Vector3f> posList, List<Vector2f> textCoordList,
		    List<Vector3f> normList, List<TriangleFace> facesList) {

		    List<Integer> indices = new ArrayList();
		    // Create position array in the order it has been declared
		    float[] posArr = new float[posList.size() * 3];
		    int i = 0;
		    for (Vector3f pos : posList) {
		        posArr[i * 3] = pos.x;
		        posArr[i * 3 + 1] = pos.y;
		        posArr[i * 3 + 2] = pos.z;
		        i++;
		    }
		    float[] textCoordArr = new float[posList.size() * 2];
		    float[] normArr = new float[posList.size() * 3];

		    for (TriangleFace face : facesList) {
		        IdxGroup[] faceVertexIndices = face.getFaceVertexIndices();
		        for (IdxGroup indValue : faceVertexIndices) {
		            processFaceVertex(indValue, textCoordList, normList,
		                indices, textCoordArr, normArr);
		        }
		    }
		    int[] indicesArr = new int[indices.size()];
		    indicesArr = indices.stream().mapToInt((Integer v) -> v).toArray();
		    Mesh mesh = new Mesh(posArr, indicesArr, textCoordArr,normArr );
		    return mesh;
		}

		private static void processFaceVertex(IdxGroup indices, List<Vector2f> textCoordList,
		    List<Vector3f> normList, List<Integer> indicesList,
		    float[] texCoordArr, float[] normArr) {

		    // Set index for vertex coordinates
		    int posIndex = indices.idxPos;
		    indicesList.add(posIndex);

		    // Reorder texture coordinates
		    if (indices.idxTextCoord >= 0) {
		        Vector2f textCoord = textCoordList.get(indices.idxTextCoord);
		        texCoordArr[posIndex * 2] = textCoord.x;
		        texCoordArr[posIndex * 2 + 1] = 1 - textCoord.y;
		    }
		    if (indices.idxVecNormal >= 0) {
		        // Reorder vectornormals
		        Vector3f vecNorm = normList.get(indices.idxVecNormal);
		        normArr[posIndex * 3] = vecNorm.x;
		        normArr[posIndex * 3 + 1] = vecNorm.y;
		        normArr[posIndex * 3 + 2] = vecNorm.z;
		    }
		   
		}
		
	protected static class IdxGroup {
	
		    public static final int NO_VALUE = -1;
	
		    public int idxPos;
	
		    public int idxTextCoord;
	
		    public int idxVecNormal;
	
		    public IdxGroup() {
		        idxPos = NO_VALUE;
		        idxTextCoord = NO_VALUE;
		        idxVecNormal = NO_VALUE;
		        }
		}
	protected static class QuadFace{
		private IdxGroup[] idxGroups = new IdxGroup[4];
		String s1;
		String s2;
		String s3;
		String s4;
		public QuadFace(String v1, String v2, String v3,String v4) {
			 s1 = v1;
			 s2 = v2;
			 s3 = v3;
			 s4 = v4;

	        idxGroups = new IdxGroup[4];
	        // Parse the lines
	        idxGroups[0] = parseLine(v1);
	        idxGroups[1] = parseLine(v2);
	        idxGroups[2] = parseLine(v3);
	        idxGroups[3] = parseLine(v4);
	    }
		private IdxGroup parseLine(String line) {
	        IdxGroup idxGroup = new IdxGroup();

	        String[] lineTokens = line.split("/");
	        int length = lineTokens.length;
	        idxGroup.idxPos = Integer.parseInt(lineTokens[0]) - 1;
	        if (length > 1) {
	            // It can be empty if the obj does not define text coords
	            String textCoord = lineTokens[1];
	            idxGroup.idxTextCoord = textCoord.length() > 0 ? Integer.parseInt(textCoord) - 1 : IdxGroup.NO_VALUE;
	            if (length > 2) {
	                idxGroup.idxVecNormal = Integer.parseInt(lineTokens[2]) - 1;
	            }
	        }

	        return idxGroup;
	    }
		public TriangleFace[] ToTriangle(QuadFace Qface){
			TriangleFace T1 = new TriangleFace(s1,s2,s3);
			TriangleFace T2 = new TriangleFace(s3,s1,s4);
			 TriangleFace[] tr = new TriangleFace[] {T1,T2};
			 return tr;
		}

		
	}
	protected static class TriangleFace {
	
		    /**
		     * List of idxGroup groups for a face triangle (3 vertices per face).
		    */
		    private IdxGroup[] idxGroups = new IdxGroup[3];

		    public TriangleFace(String v1, String v2, String v3) {
		        idxGroups = new IdxGroup[3];
		        // Parse the lines
		        idxGroups[0] = parseLine(v1);
		        idxGroups[1] = parseLine(v2);
		        idxGroups[2] = parseLine(v3);
		    }

		    private IdxGroup parseLine(String line) {
		        IdxGroup idxGroup = new IdxGroup();
	
		        String[] lineTokens = line.split("/");
		        int length = lineTokens.length;
		        idxGroup.idxPos = Integer.parseInt(lineTokens[0]) - 1;
		        if (length > 1) {
		            // It can be empty if the obj does not define text coords
		            String textCoord = lineTokens[1];
		            idxGroup.idxTextCoord = textCoord.length() > 0 ? Integer.parseInt(textCoord) - 1 : IdxGroup.NO_VALUE;
		            if (length > 2) {
		                idxGroup.idxVecNormal = Integer.parseInt(lineTokens[2]) - 1;
		            }
		        }
	
		        return idxGroup;
		    }
	
		    public IdxGroup[] getFaceVertexIndices() {
		        return idxGroups;
		    }
		}
	

}
