#pragma once
//------------------------------------------------------------------------------
/**
	@file scripting/api/game.h

	Contains api calls to various game system functions

	(C) 2019 Individual contributors, see AUTHORS file
*/
//------------------------------------------------------------------------------
#include "nsharp/nsconfig.h"

namespace Scripting
{

namespace Api
{

//------------------------------------------------------------------------------
/**
	Check whether an entity id is valid.
*/
NEBULA_EXPORT bool EntityIsValid(uint32_t worldId, uint32_t entity);

//------------------------------------------------------------------------------
/**
*/
NEBULA_EXPORT uint32_t EntityCreateFromTemplate(uint32_t worldId, const char* tmpl);

//------------------------------------------------------------------------------
/**
*/
NEBULA_EXPORT void EntityDelete(uint32_t worldId, uint32_t entity);

//------------------------------------------------------------------------------
/**
*/
NEBULA_EXPORT bool EntityHasComponent(uint32_t worldId, uint32_t entity, uint32_t componentId);

//------------------------------------------------------------------------------
/**
*/
NEBULA_EXPORT uint32_t ComponentGetId(const char* name);

//------------------------------------------------------------------------------
/**
*/
NEBULA_EXPORT void ComponentGetData(uint32_t worldId, uint32_t entity, uint32_t componentId, void* outData, int dataSize);

//------------------------------------------------------------------------------
/**
*/
NEBULA_EXPORT void ComponentSetData(uint32_t worldId, uint32_t entity, uint32_t componentId, void* data, int dataSize);

//------------------------------------------------------------------------------
/**
*/
NEBULA_EXPORT uint32_t WorldGetDefaultWorldId();

} // namespace Api

} // namespace Scripting
