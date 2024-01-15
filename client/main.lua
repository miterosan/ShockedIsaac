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

function mod:onIsaacDamage(aEntity, DamageAmount, DamageFlags, DamageSource, DamageCountdown)

    local isIntentional = (DamageFlags & DamageFlag.DAMAGE_NO_PENALTIES) == DamageFlag.DAMAGE_NO_PENALTIES

    local amount = tostring(tonumber(string.format(DamageAmount, "%.f")))

    if isIntentional then
        mod:sendMessage("onIntentionalDamage," .. amount)
    else
        mod:sendMessage("onDamage," .. amount)
    end
    
    return nil
end



mod:AddCallback(ModCallbacks.MC_ENTITY_TAKE_DMG, mod.onIsaacDamage, EntityType.ENTITY_PLAYER)
