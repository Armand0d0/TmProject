package engine;

import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;


public class Utils {

    public static String loadResource(String fileName) throws Exception {
        String result;
        try (InputStream in = Class.forName(Utils.class.getName()).getResourceAsStream(fileName);
			 Scanner scanner = new Scanner(in, "UTF-8")) {
            result = scanner.useDelimiter("\\A").next();
        }
        return result;
    }
	public static String getResource(String filename){
		StringBuilder string = new StringBuilder();
		BufferedReader br;
		try{
			br = new BufferedReader(new FileReader(new File("./gradle/src/main/resources/" + filename)));
			String line;
			while((line = br.readLine())!=null){
				string.append(line);
				string.append("\n");
			}
			br.close();
		}catch(IOException e){
			e.printStackTrace();
		}
		return string.toString();
	}
	public static String getShader(String fileName)throws Exception{
		return loadResource("/shaders/"+fileName);
	}
	public static String getTexture(String fileName)throws Exception{
		return loadResource("/textures/"+fileName);
	}
	public static List<String> getModel(String fileName)throws Exception{
		return readAllLines("/models/"+fileName);
	}
	 public static List<String> readAllLines(String fileName) throws Exception {
	        List<String> list = new ArrayList<>();
	        BufferedReader br;
			try{
				br = new BufferedReader(new InputStreamReader(Class.forName(Utils.class.getName()).getResourceAsStream(fileName)));
				String line;
				while((line = br.readLine())!=null){
					list.add(line);
				}
				br.close();
			}catch(IOException e){
				e.printStackTrace();
			}
			return list;
	}
	 

	 
	 
	 
	 
	 
	 
	 
	 
	 
	 
}