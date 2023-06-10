discord-hook-server-start = Сервер запущен!
discord-hook-round-start = Раунд #{$roundid} начался на карте {$mapname}. <t:{$timestamp}:R>
discord-hook-round-end = Раунд #{$roundid} закончился. Он длился {$hours}:{$minutes} часов.
discord-hook-player-ban-admin =
    Игрок **`{$player}`** был забанен
    	Причина: {$reason}
    	Истекает: { $timestamp ->
            [never] Никогда.
           *[other] <t:{$timestamp}:R>
        }
    	Администратор: {$admin}
discord-hook-player-ban-none =
    Игрок **`{$player}`** был забанен
    	Причина: {$reason}
    	Истекает: { $timestamp ->
            [never] Никогда.
           *[other] <t:{$timestamp}:R>
        }
discord-hook-map-unknown = Неизвестно