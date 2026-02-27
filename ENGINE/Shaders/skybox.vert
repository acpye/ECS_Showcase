#version 330

layout (location = 0) in vec3 a_Position;
layout (location = 1) in vec2 a_TexCoord;

uniform mat4 ModelViewProjMat;

out vec2 v_TexCoord;

void main()
{
    // Apply model-view-projection transformation
    vec4 pos = ModelViewProjMat * vec4(a_Position, 1.0);
    
    // Set z = w to ensure the skybox is always at the far clip plane
    gl_Position = pos.xyww;
    
    v_TexCoord = a_TexCoord;
}