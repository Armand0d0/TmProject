package graphic;

import de.matthiasmann.twl.utils.PNGDecoder;
import de.matthiasmann.twl.utils.PNGDecoder.Format;
import engine.Utils;
import org.lwjgl.system.MemoryStack;
import static org.lwjgl.stb.STBImage.*;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.nio.ByteBuffer;
import java.nio.IntBuffer;
import java.nio.file.Paths;


import static org.lwjgl.opengl.GL11.*;
import static org.lwjgl.opengl.GL30.glGenerateMipmap;

public class Texture {

	private final int id;
    static int Width;
    static int Height;
    static int width;
    static int height;
//    public Texture(String fileName) throws Exception {
//
//        this(loadTexture("/textures/"+fileName));
//    }

    public Texture(int id) {
        this.id = id;


    }

    public void bind() {
        glBindTexture(GL_TEXTURE_2D, id);
    }

    public int getId() {
        return id;
    }

    public Texture(String fileName) throws Exception {
        ByteBuffer buf;
        // Load Texture file
        try (MemoryStack stack = MemoryStack.stackPush()) {
            IntBuffer w = stack.mallocInt(1);
            IntBuffer h = stack.mallocInt(1);
            IntBuffer channels = stack.mallocInt(1);

            URL url = Texture.class.getResource( "/textures"+ fileName);
            if (url == null) { throw new Exception("url = null (File not found)");}
                File file = Paths.get(url.toURI()).toFile();
                String filePath = file.getAbsolutePath();
                buf = stbi_load(filePath, w, h, channels, 4);
                if (buf == null) {
                    throw new Exception("Image file [" + filePath + "] not loaded: " + stbi_failure_reason());
                }

            width = w.get();
            height = h.get();
            Width = width;
            Height = height;

        }

        this.id = createTexture(buf);

        stbi_image_free(buf);

    }
    private int createTexture(ByteBuffer buf) {
        // Create a new OpenGL texture
        int textureId = glGenTextures();
        // Bind the texture
        glBindTexture(GL_TEXTURE_2D, textureId);

        // Tell OpenGL how to unpack the RGBA bytes. Each component is 1 byte size
        glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

        // Upload the texture data
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0,
                GL_RGBA, GL_UNSIGNED_BYTE, buf);
        // Generate Mip Map
        glGenerateMipmap(GL_TEXTURE_2D);

        return textureId;
    }
    public Texture(ByteBuffer imageBuffer) throws Exception {
        ByteBuffer buf;
        // Load Texture file
        try (MemoryStack stack = MemoryStack.stackPush()) {
            IntBuffer w = stack.mallocInt(1);
            IntBuffer h = stack.mallocInt(1);
            IntBuffer channels = stack.mallocInt(1);

            buf = stbi_load_from_memory(imageBuffer, w, h, channels, 4);
            if (buf == null) {
                throw new Exception("Image file not loaded: " + stbi_failure_reason());
            }


            width = w.get();
            height = h.get();
        }

        this.id = createTexture(buf);

        stbi_image_free(buf);
    }
//    private static int loadTexture(String fileName) throws Exception {
//    	InputStream in = Class.forName(Utils.class.getName()).getResourceAsStream(fileName);
//        // Load Texture file
//        PNGDecoder decoder = new
//                PNGDecoder
//                (in);
//
//        // Load texture contents into a byte buffer
//        ByteBuffer buf = ByteBuffer.allocateDirect(
//                4 * decoder.getWidth() * decoder.getHeight());
//        decoder.decode(buf, decoder.getWidth() * 4, Format.RGBA);
//        buf.flip();
//        width = decoder.getWidth();
//        height = decoder.getHeight();
//        // Create a new OpenGL texture
//        int textureId = glGenTextures();
//        // Bind the texture
//        glBindTexture(GL_TEXTURE_2D, textureId);
//
//        // Tell OpenGL how to unpack the RGBA bytes. Each component is 1 byte size
//        glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
//
//       glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
//        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
//
//        // Upload the texture data
//        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, decoder.getWidth(), decoder.getHeight(), 0,
//                GL_RGBA, GL_UNSIGNED_BYTE, buf);
//        // Generate Mip Map
//        glGenerateMipmap(GL_TEXTURE_2D);
//        return textureId;
//    }

    public void cleanup() {
        glDeleteTextures(id);
}

    public int GetWidth() { return this.width; }

    public int GetHeight() {
        return this.height;
    }
    public int getWidth() {
        return this.Width;
    }

    public int getHeight() {
        return this.Height;
    }
}
