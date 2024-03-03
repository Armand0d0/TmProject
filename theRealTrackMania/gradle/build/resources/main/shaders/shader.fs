#version 330


in vec2 outTexCoord;
in vec3 mvVertexNormal;
in vec3 mvVertexPos;
in vec4 mvPos;
vec3 FragPos = mvPos.xyz;
out vec4 fragColor;

uniform vec3 colour;
uniform int useColour;
uniform vec3 camera_Pos;
uniform sampler2D texture_sampler;
vec4 texture = texture(texture_sampler, outTexCoord);

struct PointLight{
vec3 pos;
vec3 colour;
float intensity;

};
struct SpotLight{
PointLight pl;
float angle;
vec3 direction;

};
struct DirectionalLight{
vec3 pos;
vec3 colour;
float intensity;

};
struct Material
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float reflectance;
};
uniform Material material;
 vec3 ambientC;
 vec3 diffuseC;
 vec3 speculrC;

void setupColours(Material material)
{
       ambientC = material.ambient;
       diffuseC = material.diffuse;
       speculrC = material.specular;

}

const int MAX_PL = 5;
const int MAX_DL = 5;
const int MAX_SL = 5;
uniform PointLight pointLights[MAX_PL];
uniform SpotLight spotLights[MAX_SL];
uniform DirectionalLight directionalLights[MAX_DL];


vec3 calcLightDiffuse(vec3 fromLight,vec3 colour ,float intensity,vec3 normal){
float factor =  max(dot(normalize(fromLight),normal),0.0);
vec3 diffuse = diffuseC * colour * intensity * factor;
return diffuse;
}
vec3 calcLightSpecular(vec3 fromLight,vec3 colour ,float intensity,vec3 normal){
vec3 camera_direction = normalize(camera_Pos - FragPos);
    vec3 reflected_light = normalize(reflect(fromLight , normal));
    float specularFactor = max( dot(camera_direction, reflected_light), 0.0);
    float specularPower = 100;
   specularFactor = pow(specularFactor, specularPower);
  vec3  Specular = speculrC * intensity  * material.reflectance * specularFactor * colour;


 return Specular;
}

vec3 clacPointLight(PointLight pointLight){


vec3 fromLight = FragPos-pointLight.pos;

float Dist = length(fromLight);

vec3 DiffuseLightColour = calcLightDiffuse(fromLight,pointLight.colour,pointLight.intensity,-mvVertexNormal);
vec3 SpecularLightColour = calcLightSpecular(fromLight,pointLight.colour,pointLight.intensity,-mvVertexNormal);
vec3 LightColour = DiffuseLightColour + SpecularLightColour;

float att = 10+0 * Dist +1*Dist*Dist;
return LightColour/att;

}
vec3 clacDirLight(DirectionalLight dirLight){
vec3 DiffuseLightColour = calcLightDiffuse(dirLight.pos,dirLight.colour,dirLight.intensity,mvVertexNormal);
vec3 SpecularLightColour = calcLightSpecular(-dirLight.pos,dirLight.colour,dirLight.intensity,mvVertexNormal);
return DiffuseLightColour + SpecularLightColour;
}
vec3 clacSpotLight(SpotLight spotLight){
vec3 light_direction = spotLight.pl.pos - FragPos;
    vec3 to_light_dir  = normalize(light_direction);
    vec3 from_light_dir = -to_light_dir;
    float spot_alfa = dot(from_light_dir, normalize(spotLight.direction));
   //vec4 light_colour = calcLightColour(light.colour, light.pl.intensity, position, to_light_dir, normal);
	vec3 colour = vec3(0, 0, 0);

	if ( spot_alfa > radians(spotLight.angle) ) {
        colour = clacPointLight(spotLight.pl);//, FragPos, mvVertexNormal);
        colour *= (1.0 - (1.0 - spot_alfa)/(1.0 - spotLight.angle));

}
return colour;
}
void main()
{
    setupColours(material);
    vec3 ambient = vec3(0.2) * ambientC;
    vec3 diffuseSpec=vec3(0,0,0);
    for(int i=0;i<MAX_PL;i++){
            diffuseSpec += clacPointLight(pointLights[i]);
    }
    for(int i=0;i<MAX_DL;i++){
             diffuseSpec += clacDirLight(directionalLights[i]);
    }
    for(int i=0;i<MAX_SL;i++){
             diffuseSpec += clacSpotLight(spotLights[i]);
    }
  fragColor = texture * vec4(ambient,1) + vec4(diffuseSpec,1);
}