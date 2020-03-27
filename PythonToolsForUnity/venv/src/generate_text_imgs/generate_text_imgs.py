import sys,re,pyttsx3
from PIL import Image, ImageDraw, ImageFont

marginPercent = 0.1
spacePercent = 0.07


def generate():
    imgNames = open('ToGenerateImgNames.txt')

    for line in imgNames.readlines():
        line = line.strip('\n')
        if line.endswith("jpg") or line.endswith("png"):
            createImg(line)

    engine = pyttsx3.init()

    engine.say("文字转语音")
    engine.runAndWait()
    # 朗读一次
    engine.endLoop()


def createImg(imgLine):
    tmpArr = imgLine.split('.')
    postfix = tmpArr[len(tmpArr)-1]
    arr = re.split(r"[\[x\]-]",imgLine)
    x = int(arr[len(arr) - 3])
    y = int(arr[len(arr) - 2])

    if postfix == 'jpg':
        mode = 'RGB'
    else:
        mode = "RGBA"

    img = Image.new(mode, (x, y), (252, 246, 239, 255))
    d = ImageDraw.Draw(img)
    lineCount = len(arr) -3

    heightPerLine = ((1 - marginPercent*2)/lineCount)

    for i in range(0, len(arr)-3):
        yStart = (marginPercent + i*heightPerLine) * y
        charNum = len(arr[i])
        charWidth = (1 - marginPercent*2)/charNum * x
        charBlockSize = min(charWidth, heightPerLine*y)
        fontSize = int((1- spacePercent*2)*charBlockSize*1.2)
        xStart = (x - charBlockSize*charNum)/2
        ft = ImageFont.truetype('../Fonts/FZYDZHJW.ttf', fontSize)
        d.text((xStart, yStart), arr[i], font=ft, fill=(147, 129, 107, 255))

    if postfix == 'jpg':
        format = "JPEG"
    else:
        format = "PNG"



    img.save("../../../../../GeneratedArtworks/" + clearContentWithSpecialCharacter(imgLine), format)


def clearContentWithSpecialCharacter(s):
    s = s.replace("[","$")
    s = s.replace("]","$")
    pattern = re.compile(r'(\$)(.*)(\$)')
    return pattern.sub(r'',s)


generate()