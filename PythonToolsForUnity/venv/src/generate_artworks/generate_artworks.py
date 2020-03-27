import sys,re,math
from PIL import Image, ImageDraw, ImageFont
from aip import AipSpeech

marginPercent = 0.1
spacePercent = 0.07
APP_ID = '14279390'
API_KEY = '3jWdhQmw5MjysfkpUWXj5afo'
SECRET_KEY = 'VHP79nZZlsCUEB8iGNR9wqU0G9lHdAjG'
speechClient = AipSpeech(APP_ID, API_KEY, SECRET_KEY)
minFontSize = 20

charCountPerSecond = 4



def generate():
    artworkListFileName = sys.argv[1]
    imgSaveFolder = sys.argv[2]
    audioSaveFolder = sys.argv[3]
    fontPath = sys.argv[4]

    artworkNames = open(artworkListFileName)

    for line in artworkNames.readlines():
        line = line.strip('\n')
        if line.endswith("jpg") or line.endswith("png"):
            createImg(line, imgSaveFolder, fontPath)
        elif line.endswith("mp3"):
            createAudio(line, audioSaveFolder)





def createAudio(audioLine, audioSaveFolder):
    arr = re.split(r"[\[\]-]", audioLine)
    audioLength = int(arr[len(arr) - 2])
    speechStr = clearContentWithSpecialCharacter(audioLine).replace('.mp3', '').replace('_', ' ').replace('-', ' ')

    rounds = int(audioLength * charCountPerSecond / len(speechStr))
    rounds = max(1, rounds)
    for i in range(1, rounds):
        speechStr += speechStr

    result = speechClient.synthesis(speechStr, 'zh', 1, {
        'vol': 5, 'per': charCountPerSecond
    })

    if not isinstance(result, dict):
        with open(audioSaveFolder + "/" + clearContentWithSpecialCharacter(audioLine), 'wb') as f:
            f.write(result)



def createImg(imgLine, imgSaveFolder, fontPath):
    tmpArr = imgLine.split('.')
    postfix = tmpArr[len(tmpArr)-1]
    arr = re.split(r"[\[x\]-]",imgLine)
    x = int(arr[len(arr) - 3])
    y = int(arr[len(arr) - 2])

    if postfix == 'jpg':
        mode = 'RGB'
    else:
        mode = "RGBA"


    img = Image.new(mode, (x, y), (66, 116, 98, 255))
    d = ImageDraw.Draw(img)
    d.rectangle([(marginPercent*0.5*x, marginPercent*0.5*y), ((1-marginPercent*0.5)*x, (1-marginPercent*0.5)*y)], (252, 246, 239, 255))

    fillPercent = 1-2*marginPercent
    charCount = len(arr[0] + arr[1])
    charWidth = max(minFontSize, int(math.sqrt((x*fillPercent*y*fillPercent)/charCount)))
    charCountPerLine = int(x * fillPercent/ charWidth)

    lines = []


    i = 0
    while i < len(arr[1]):
        lines.append(arr[1][i: min(i+charCountPerLine, len(arr[1]))])
        i = i + charCountPerLine

    englishWords = arr[0].split('_')
    for j in range(0, len(englishWords)):
        lines.append(englishWords[j])

    lineCount = len(lines)
    heightPerLine = ((1 - marginPercent*2)/lineCount)*y
    fontSize = min(charWidth, int(heightPerLine * fillPercent))
    index = 0
    ft = ImageFont.truetype(fontPath, fontSize)

    while index < lineCount:
        yStart = int(marginPercent* y + heightPerLine*index)
        line = lines[index]
        xStart = int(marginPercent*x)
        d.text((xStart, yStart), line, font=ft, fill=(147, 129, 107, 255))
        index = index + 1

    if postfix == 'jpg':
        format = "JPEG"
    else:
        format = "PNG"

    img.save(imgSaveFolder + "/" + clearContentWithSpecialCharacter(imgLine), format)


def clearContentWithSpecialCharacter(s):
    s = s.replace("[","$")
    s = s.replace("]","$")
    pattern = re.compile(r'(\$)(.*)(\$)')
    return pattern.sub(r'',s)






generate()