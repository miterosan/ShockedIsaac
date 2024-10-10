local mod = RegisterMod("ShockedIsaac", 1)
local socket = require("socket")

-- would it be better to not reconnect every time a message gets send?
function mod:sendMessage(message)
    local host, port = "localhost", 11000
    local tcp = assert(socket.tcp())

    tcp:connect(host, port)

    tcp:send(message)

    tcp:close()
end

function mod:getPlayerIndexByControllerIndex(controllerIndex)
    local game = Game()
    local numPlayers = game:GetNumPlayers()

    for i = 0, numPlayers - 1, 1 do
        local p = Isaac.GetPlayer(i)

        if p.ControllerIndex == controllerIndex then 
            return i
        end
    end

    return -1
end

function mod:onIsaacDamage(aEntity, DamageAmount, DamageFlags, DamageSource, DamageCountdown)

    local isIntentional = (DamageFlags & DamageFlag.DAMAGE_NO_PENALTIES) == DamageFlag.DAMAGE_NO_PENALTIES

    local amount = string.format("%.f", DamageAmount)

    local player = aEntity:ToPlayer()
    local playerIndex = mod:getPlayerIndexByControllerIndex(player.ControllerIndex)

    if isIntentional then
        mod:sendMessage("onIntentionalDamage," .. amount .. "," .. playerIndex)
    else
        mod:sendMessage("onDamage," .. amount .. "," .. playerIndex)
    end

    return nil
end

function mod:onIsaacDeath(entity)

    local player = entity:ToPlayer()
    local playerIndex = mod:getPlayerIndexByControllerIndex(player.ControllerIndex)

    mod:sendMessage("onDeath," .. playerIndex)
end


mod:AddCallback(ModCallbacks.MC_POST_ENTITY_KILL, mod.onIsaacDeath, EntityType.ENTITY_PLAYER)
mod:AddCallback(ModCallbacks.MC_ENTITY_TAKE_DMG, mod.onIsaacDamage, EntityType.ENTITY_PLAYER)
