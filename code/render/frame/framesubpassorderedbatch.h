#pragma once
//------------------------------------------------------------------------------
/**
	A subpass sorted batch renders the same geometry as the ordinary batch, however
	it prioritizes Z-order instead shader, making it potentially detrimental for performance.
	
	(C) 2016 Individual contributors, see AUTHORS file
*/
//------------------------------------------------------------------------------
#include "frameop.h"
#include "coregraphics/batchgroup.h"
namespace Frame
{
class FrameSubpassOrderedBatch : public FrameOp
{
public:
	/// constructor
	FrameSubpassOrderedBatch();
	/// destructor
	virtual ~FrameSubpassOrderedBatch();

	/// run operation
	void Run(const IndexT frameIndex);

	CoreGraphics::BatchGroup::Code batch;
};

} // namespace Frame2