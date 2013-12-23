using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// * \ingroup objgengrp
// * Generates a cylinder mesh along Y-axis
// * \image html primitive_cylinder.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport CylinderGenerator : public MeshGenerator<CylinderGenerator>
public class CylinderGenerator : MeshGenerator<CylinderGenerator>
{
	private uint mNumSegBase;
	private uint mNumSegHeight;
	private bool mCapped;
	private float mRadius = 0f;
	private float mHeight = 0f;

	/// Contructor with arguments
	public CylinderGenerator(float radius, float height, uint numSegBase, uint numSegHeight) : this(radius, height, numSegBase, numSegHeight, true)
	{
	}
	public CylinderGenerator(float radius, float height, uint numSegBase) : this(radius, height, numSegBase, 1, true)
	{
	}
	public CylinderGenerator(float radius, float height) : this(radius, height, 16, 1, true)
	{
	}
	public CylinderGenerator(float radius) : this(radius, 1.0f, 16, 1, true)
	{
	}
	public CylinderGenerator() : this(1.0f, 1.0f, 16, 1, true)
	{
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: CylinderGenerator(Ogre::float radius = 1.0f, Ogre::float height = 1.0f, uint numSegBase = 16, uint numSegHeight = 1, bool capped = true) : mNumSegBase(numSegBase), mNumSegHeight(numSegHeight), mCapped(capped), mRadius(radius), mHeight(height)
	public CylinderGenerator(float radius, float height, uint numSegBase, uint numSegHeight, bool capped)
	{
		mNumSegBase = numSegBase;
		mNumSegHeight = numSegHeight;
		mCapped = capped;
		mRadius = radius;
		mHeight = height;
	}

//    *
//	 * Builds the mesh into the given TriangleBuffer
//	 * @param buffer The TriangleBuffer on where to append the mesh.
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
	public void addToTriangleBuffer(ref TriangleBuffer buffer)
	{
		buffer.rebaseOffset();
		if (mCapped)
		{
			buffer.estimateVertexCount((mNumSegHeight+1)*(mNumSegBase+1)+2*(mNumSegBase+1)+2);
			buffer.estimateIndexCount(mNumSegHeight*(mNumSegBase+1)*6+6 *mNumSegBase);
		}
		else
		{
			buffer.estimateVertexCount((mNumSegHeight+1)*(mNumSegBase+1));
			buffer.estimateIndexCount(mNumSegHeight*(mNumSegBase+1)*6);
		}
	
	
		float deltaAngle = (Math.TWO_PI / mNumSegBase);
		float deltaHeight = mHeight/(float)mNumSegHeight;
		int offset = 0;
	
		for (uint i = 0; i <=mNumSegHeight; i++)
			for (uint j = 0; j<=mNumSegBase; j++)
			{
				float x0 = mRadius * cosf(j *deltaAngle);
				float z0 = mRadius * sinf(j *deltaAngle);
	
				addPoint(buffer, new Vector3(x0, i *deltaHeight, z0), new Vector3(x0, 0, z0).normalisedCopy(), new Vector2(j/(float)mNumSegBase, i/(float)mNumSegHeight));
	
				if (i != mNumSegHeight)
				{
					buffer.index(offset + mNumSegBase + 1);
					buffer.index(offset);
					buffer.index(offset + mNumSegBase);
					buffer.index(offset + mNumSegBase + 1);
					buffer.index(offset + 1);
					buffer.index(offset);
				}
				offset ++;
			}
		if (mCapped)
		{
			//low cap
			int centerIndex = offset;
			addPoint(buffer, Vector3.ZERO, Vector3.NEGATIVE_UNIT_Y, Vector2.ZERO);
			offset++;
			for (uint j =0; j<=mNumSegBase; j++)
			{
				float x0 = cosf(j *deltaAngle);
				float z0 = sinf(j *deltaAngle);
	
				addPoint(buffer, new Vector3(mRadius *x0, 0.0f, mRadius *z0), Vector3.NEGATIVE_UNIT_Y, new Vector2(x0, z0));
				if (j!=mNumSegBase)
				{
					buffer.index(centerIndex);
					buffer.index(offset);
					buffer.index(offset+1);
				}
				offset++;
			}
			// high cap
			centerIndex = offset;
			addPoint(buffer, new Vector3(0, mHeight, 0), Vector3.UNIT_Y, Vector2.ZERO);
			offset++;
			for (uint j =0; j<=mNumSegBase; j++)
			{
				float x0 = cosf(j *deltaAngle);
				float z0 = sinf(j *deltaAngle);
	
				addPoint(buffer, new Vector3(x0 * mRadius, mHeight, mRadius * z0), Vector3.UNIT_Y, new Vector2(x0, z0));
				if (j!=mNumSegBase)
				{
					buffer.index(centerIndex);
					buffer.index(offset+1);
					buffer.index(offset);
				}
				offset++;
			}
		}
	}

//    *
//	Sets the number of segments when rotating around the cylinder's axis (default=16)
//	\exception Ogre::InvalidParametersException Minimum of numSegBase is 1
//	
	public CylinderGenerator setNumSegBase(uint numSegBase)
	{
		if (numSegBase == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::CylinderGenerator::setNumSegBase(unsigned int)", __FILE__, __LINE__);
			 throw new Exception("numSegBase must be larger than 0!");
            ;
		mNumSegBase = numSegBase;
		return this;
	}

//    *
//	Sets the number of segments along the height of the cylinder (default=1)
//	\exception Ogre::InvalidParametersException Minimum of numSegHeight is 1
//	
	public CylinderGenerator setNumSegHeight(uint numSegHeight)
	{
		if (numSegHeight == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::CylinderGenerator::setNumSegHeight(unsigned int)", __FILE__, __LINE__);
			 throw new Exception("numSegHeight must be larger than 0!");
            ;
		mNumSegHeight = numSegHeight;
		return this;
	}

	//* Sets whether the cylinder has endings or not (default=true) 
	public CylinderGenerator setCapped(bool capped)
	{
		mCapped = capped;
		return this;
	}

//    *
//	Sets the radius of the cylinder (default=1)
//	\exception Ogre::InvalidParametersException Radius must be larger than 0!
//	
	public CylinderGenerator setRadius(float radius)
	{
		if (radius <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::CylinderGenerator::setRadius(Ogre::Real)", __FILE__, __LINE__);
			 throw new Exception("radius must be larger than 0!");
            ;
		mRadius = radius;
		return this;
	}

//    *
//	Sets the height of the cylinder (default=1)
//	\exception Ogre::InvalidParametersException Height must be larger than 0!
//	
	public CylinderGenerator setHeight(float height)
	{
		if (height <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Height must be larger than 0!", "Procedural::CylinderGenerator::setHeight(Ogre::Real)", __FILE__, __LINE__);
            throw new Exception("height must be larger than 0!");
            ;
		mHeight = height;
		return this;
	}

}
}


