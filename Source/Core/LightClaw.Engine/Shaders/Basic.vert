﻿#version 400

uniform mat4 modelViewProjectionMatrix;
uniform sampler2D texture;

in vec3 inVertexPosition;
in vec2 inTextureCoordinates;

out vec2 passTextureCoordinates;

void main(void)
{
	gl_Position = modelViewProjectionMatrix * inVertexPosition;
	passTextureCoordinates = inTextureCoordinates;
}