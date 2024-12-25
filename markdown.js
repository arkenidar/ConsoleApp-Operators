var showdownScript=document.createElement('script')
///showdownScript.src='https://cdn.jsdelivr.net/npm/showdown@2.1.0/dist/showdown.min.js'
showdownScript.src='https://arkenidar.com/app/lib/html-markdown/showdown.min.js'

var scriptNode
scriptNode=document.currentScript
scriptNode.insertAdjacentElement('afterend',showdownScript)

addEventListener('load',setupMarkdownViewers)

function setupMarkdownViewers(){

	var showdownConverter = new showdown.Converter()

	function viewContent(content,node){
		node.insertAdjacentElement('afterend', document.createElement("div") )
		node.style.display = "none"
		var newNode = node.nextSibling
		newNode.innerHTML=showdownConverter.makeHtml(content)
	}

	for(var node of document.querySelectorAll('.markdown-inline')){
		node.style.whiteSpace='pre'
		var markdown=node.innerText
		node.style.whiteSpace='normal'
		viewContent(markdown,node)
	}

	function markdownContentFromURL(url,node){
		fetch(url)
		.then(response=>response.text())
		.then(text=>viewContent(text,node))
	}
	
	for(var node of document.querySelectorAll('.markdown-url'))
		markdownContentFromURL(node.dataset.url,node)
}
