#pragma once
//------------------------------------------------------------------------------
/**
	A FrameScript describes render operations being done to produce a single frame.

	Frame scripts can also execute a subset of commands, which is retrievable through 
	an execution mask.
	
	(C) 2016 Individual contributors, see AUTHORS file
*/
//------------------------------------------------------------------------------
#include "core/refcounted.h"
#include "coregraphics/rendertexture.h"
#include "coregraphics/shaderrwtexture.h"
#include "coregraphics/shaderrwbuffer.h"
#include "coregraphics/event.h"
#include "coregraphics/shader.h"
#include "algorithm/algorithm.h"
#include "frame/frameop.h"
#include "frame/framepass.h"
#include "memory/chunkallocator.h"
namespace Graphics
{
	class View;
}
namespace Frame
{
class FrameScript : public Core::RefCounted
{
	__DeclareClass(FrameScript);
public:
	/// constructor
	FrameScript();
	/// destructor
	virtual ~FrameScript();

	/// get allocator
	Memory::ChunkAllocator<0xFFFF>& GetAllocator();

	/// set name
	void SetResourceName(const Resources::ResourceName& name);
	/// get name
	const Resources::ResourceName& GetResourceName() const;

	/// add frame operation
	void AddOp(Frame::FrameOp* op);
	/// add color texture
	void AddColorTexture(const Util::StringAtom& name, const CoreGraphics::RenderTextureId tex);
	/// get color texture
	const CoreGraphics::RenderTextureId GetColorTexture(const Util::StringAtom& name);
	/// add depth-stencil texture
	void AddDepthStencilTexture(const Util::StringAtom& name, const CoreGraphics::RenderTextureId tex);
	/// get depth-stencil texture
	const CoreGraphics::RenderTextureId GetDepthStencilTexture(const Util::StringAtom& name);
	/// add read-write texture
	void AddReadWriteTexture(const Util::StringAtom& name, const CoreGraphics::ShaderRWTextureId tex);
	/// get read-write texture
	const CoreGraphics::ShaderRWTextureId GetReadWriteTexture(const Util::StringAtom& name);
	/// add read-write buffer
	void AddReadWriteBuffer(const Util::StringAtom& name, const CoreGraphics::ShaderRWBufferId buf);
	/// get read-write buffer
	const CoreGraphics::ShaderRWBufferId GetReadWriteBuffer(const Util::StringAtom& name);
	/// add event
	void AddEvent(const Util::StringAtom& name, const CoreGraphics::EventId event);
	/// get event
	const CoreGraphics::EventId GetEvent(const Util::StringAtom& name);
	/// add algorithm
	void AddAlgorithm(const Util::StringAtom& name, Algorithms::Algorithm* alg);
	/// get algorithm
	Algorithms::Algorithm* GetAlgorithm(const Util::StringAtom& name);
	/// add shader state
	void AddShaderState(const Util::StringAtom& name, const CoreGraphics::ShaderStateId state);
	/// get shader state
	const CoreGraphics::ShaderStateId GetShaderState(const Util::StringAtom& name);

	/// setup script
	void Setup();
	/// discard script
	void Discard();
	/// run script
	void Run(const IndexT frameIndex);
	/// run segment of script
	void RunSegment(const FrameOp::ExecutionMask mask, const IndexT frameIndex);

	/// create an execution between two subpasses
	FrameOp::ExecutionMask CreateMask(const Util::StringAtom& startOp, const Util::StringAtom& endOp);
	/// create an execution mask running up to a pass, and then into a certain subpass
	FrameOp::ExecutionMask CreateSubpassMask(const Ptr<FramePass>& pass, const Util::StringAtom& passOp, const Util::StringAtom& subpass);
	/// get begin and end ops for mask
	void GetOps(const FrameOp::ExecutionMask mask, FrameOp* startOp, FrameOp* endOp);

private:
	friend class FrameScriptLoader;
	friend class FrameServer;
	friend class Graphics::View;

	/// cleanup script internally
	void Cleanup();
	/// handle resizing
	void OnWindowResized();

	CoreGraphics::WindowId window;
	Memory::ChunkAllocator<0xFFFF> allocator;

	Resources::ResourceName resId;
	Util::Array<CoreGraphics::RenderTextureId> colorTextures;
	Util::Dictionary<Util::StringAtom, CoreGraphics::RenderTextureId> colorTexturesByName;
	Util::Array<CoreGraphics::RenderTextureId> depthStencilTextures;
	Util::Dictionary<Util::StringAtom, CoreGraphics::RenderTextureId> depthStencilTexturesByName;
	Util::Array<CoreGraphics::ShaderRWTextureId> readWriteTextures;
	Util::Dictionary<Util::StringAtom, CoreGraphics::ShaderRWTextureId> readWriteTexturesByName;
	Util::Array<CoreGraphics::ShaderRWBufferId> readWriteBuffers;
	Util::Dictionary<Util::StringAtom, CoreGraphics::ShaderRWBufferId> readWriteBuffersByName;
	Util::Array<CoreGraphics::EventId> events;
	Util::Dictionary<Util::StringAtom, CoreGraphics::EventId> eventsByName;
	Util::Array<Frame::FrameOp*> ops;
	Util::Array<Algorithms::Algorithm*> algorithms;
	Util::Dictionary<Util::StringAtom, Algorithms::Algorithm*> algorithmsByName;
	Util::Array<CoreGraphics::ShaderStateId> shaderStates;
	Util::Dictionary<Util::StringAtom, CoreGraphics::ShaderStateId> shaderStatesByName;
};

//------------------------------------------------------------------------------
/**
*/
inline void
FrameScript::SetResourceName(const Resources::ResourceName& name)
{
	this->resId = name;
}

//------------------------------------------------------------------------------
/**
*/
inline const Resources::ResourceName&
FrameScript::GetResourceName() const
{
	return this->resId;
}

//------------------------------------------------------------------------------
/**
*/
inline Memory::ChunkAllocator<0xFFFF>&
FrameScript::GetAllocator()
{
	return this->allocator;
}

//------------------------------------------------------------------------------
/**
*/
inline const CoreGraphics::RenderTextureId
FrameScript::GetColorTexture(const Util::StringAtom& name)
{
	return this->colorTexturesByName[name];
}

//------------------------------------------------------------------------------
/**
*/
inline const CoreGraphics::RenderTextureId
FrameScript::GetDepthStencilTexture(const Util::StringAtom& name)
{
	return this->depthStencilTexturesByName[name];
}

//------------------------------------------------------------------------------
/**
*/
inline const CoreGraphics::ShaderRWTextureId
FrameScript::GetReadWriteTexture(const Util::StringAtom& name)
{
	return this->readWriteTexturesByName[name];
}

//------------------------------------------------------------------------------
/**
*/
inline const CoreGraphics::ShaderRWBufferId
FrameScript::GetReadWriteBuffer(const Util::StringAtom& name)
{
	return this->readWriteBuffersByName[name];
}

//------------------------------------------------------------------------------
/**
*/
inline const CoreGraphics::EventId
FrameScript::GetEvent(const Util::StringAtom& name)
{
	return this->eventsByName[name];
}

//------------------------------------------------------------------------------
/**
*/
inline Algorithms::Algorithm*
FrameScript::GetAlgorithm(const Util::StringAtom& name)
{
	return this->algorithmsByName[name];
}

//------------------------------------------------------------------------------
/**
*/
inline const CoreGraphics::ShaderStateId
FrameScript::GetShaderState(const Util::StringAtom& name)
{
	return this->shaderStatesByName[name];
}
} // namespace Frame2