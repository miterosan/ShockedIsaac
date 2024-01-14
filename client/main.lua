local mod = RegisterMod("ShockedIsaac", 1)
local socket = require("socket")




function mod:onIsaacDamage(aEntity ,DamageAmount ,DamageFlags ,DamageSource ,DamageCountdown)

    local host, port = "localhost", 11000
    local tcp = assert(socket.tcp())

    tcp:connect(host, port)

    local isIntentional = (DamageFlags & DamageFlag.DAMAGE_NO_PENALTIES) == DamageFlag.DAMAGE_NO_PENALTIES


    local amount = tonumber(string.format(DamageAmount, "%.f"))

    if isIntentional then
        tcp:send("onIntentionalDamage," .. tostring(amount))
    else
        tcp:send("onDamage," .. tostring(amount))
    end


    tcp:close()

    return nil


end



mod:AddCallback(ModCallbacks.MC_ENTITY_TAKE_DMG, mod.onIsaacDamage, EntityType.ENTITY_PLAYER)
