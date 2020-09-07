local Color32 = {}

function Color32.new(r, g, b, a)
	return Color.New(r / 255, g / 255, b / 255, a / 255)
end

return Color32