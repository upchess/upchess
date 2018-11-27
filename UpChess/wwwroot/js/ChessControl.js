// definidos no Index.cshtml
/*
        var playerId = @playerId;
        var mesaId = @mesaId;
        var whiteUserId = @whiteUserId;
        var white = playerId == whiteUserId;
        var estado = @estado;
        var historico = @historico;
        var myNameDiv = document.querySelector("#myName");
        var otherNameDiv = document.querySelector("#otherName");
        var playerTurnDivClass = "";
        */
var board,
    boardEl = $('#board'),
    game = new Chess(),
    squareToHighlight;

// opções
let piece_theme = '/img/chesspieces/wikipedia/{piece}.png';
let colors = ['white', 'black'];
let playerColor = white ? colors[0] : colors[1];
let otherColor = white ? colors[1] : colors[0];
let promoting = false;

var removeGreySquares = function () {
    $('#board .square-55d63').css('background', '');
};

var greySquare = function (square) {
    var squareEl = $('#board .square-' + square);

    var background = '#a9a9a9';
    if (squareEl.hasClass('black-3c85d') === true) {
        background = '#696969';
    }

    squareEl.css('background', background);
};

var removeHighlights = function () {
    //boardEl.find('.square-55d63')
    //boardEl.find('highlight-' + color)
    //    .removeClass('highlight-' + color);
    boardEl.find('.highlight-white')
        .removeClass('highlight-white');
    boardEl.find('.highlight-black')
        .removeClass('highlight-black');
};
var onMouseoverSquare = function (square, piece) {
    // get list of possible moves for this square
    var moves = game.moves({
        square: square,
        verbose: true
    });

    // exit if there are no moves available for this square
    if (moves.length === 0) return;

    // highlight the square they moused over
    greySquare(square);

    // highlight the possible squares for this piece
    for (var i = 0; i < moves.length; i++) {
        greySquare(moves[i].to);
    }
};

var onMouseoutSquare = function (square, piece) {
    removeGreySquares();
};
// do not pick up pieces if the game is over
// only pick up pieces for White
var onDragStart = function (source, piece, position, orientation) {
    if (endGame()) return false;
    if (game.in_checkmate() === true || game.in_draw() === true || // fim de jogo
        (piece.search(/^b/) !== -1 && white) || // estou mexendo na peça errada (sou branco e não posso mexer no preto)
        (piece.search(/^w/) !== -1 && !white) || // estou mexendo na peça errada (sou preto e não posso mexer no branco)
        (!white && game.turn() === colors[0].charAt(0)) || // sou preto e é a vez do branco
        (white && game.turn() === colors[1].charAt(0))) { // sou branco e é a vez do preto
        return false;
    }
};
var pullOponentMove = function () {
    makeRandomMove();
}
var makeRandomMove = function () {
    var possibleMoves = game.moves({
        verbose: true
    });

    // game over
    if (possibleMoves.length === 0) return;

    var randomIndex = Math.floor(Math.random() * possibleMoves.length);
    var move = possibleMoves[randomIndex];
    game.move(move.san);

    // highlight black's move
    removeHighlights();
    boardEl.find('.square-' + move.from).addClass('highlight-' + otherColor);
    squareToHighlight = move.to;

    // update the board to the new position
    board.position(game.fen());
};

var onDrop = function (source, target) {
    // see if the move is legal
    let move_cfg = {
        from: source,
        to: target,
        promotion: 'q'
    };
    var move = game.move(move_cfg);

    // illegal move
    if (move === null) {
        return 'snapback';
    }
    // highlight white's move
    removeHighlights();
    boardEl.find('.square-' + source).addClass('highlight-' + playerColor);
    boardEl.find('.square-' + target).addClass('highlight-' + playerColor);
    otherNameDiv.classList.add(playerTurnDivClass);
    myNameDiv.classList.remove(playerTurnDivClass);
    sendMove(move, game.pgn());
    document.querySelector('#PGN').innerHTML = game.pgn().replace(/(\d{1,3}\.)/g, '\n<span style="background-color:yellow">$&</span>');
    // jogada automática
    //window.setTimeout(pullOponentMove, 250);
};
function sendMove(move, historico) {
    const Jogada = {
        'mesaId': mesaId,
        'estado': playerId + "," + move.san,
        'historico': historico
    };
    $.ajax({
        type: 'POST',
        accepts: 'application/json',
        url: '/api/Moves',
        contentType: 'application/json',
        data: JSON.stringify(Jogada),
        error: function (jqXHR, textStatus, errorThrown) {
            alert('here');
        },
        success: function (result) {
            // inicia busca da jogada do oponente em 30 segundos.
            setTimeout(getMove, 5000);
        }
    });
}
function getMove() {
    if (endGame()) return;
    $.ajax({
        type: 'GET',
        accepts: 'application/json',
        url: '/api/Moves?MesaId=' + mesaId + '&PlayerId=' + playerId,
        contentType: 'application/json',
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.responseText == '1') {
                // o retorno nesta forma acaba gerando muitas mensagens de erro na console do navegador
                // ainda aguardando outro jogador, atualizar tempo na tela
                setTimeout(getMove, 5000);
            } else {
                // ocorreu um erro.
                setTimeout(getMove, 5000);
            }
        },
        success: function (result) {
            if (result && result.historico) {
                let partes = result.estado.split(',');
                if (partes[0] == playerId) {
                    setTimeout(getMove, 5000);
                    return;
                }
                // executa a jogada
                let move = game.move(partes[1]);

                // highlight black's move
                removeHighlights();
                boardEl.find('.square-' + move.from).addClass('highlight-' + otherColor);
                squareToHighlight = move.to;
                myNameDiv.classList.add(playerTurnDivClass);
                otherNameDiv.classList.remove(playerTurnDivClass);

                // update the board to the new position
                board.position(game.fen());
                document.querySelector('#PGN').innerHTML = game.pgn().replace(/(\d{1,3}\.)/g, '\n<span style="background-color:yellow">$&</span>');
            } else {
                // ainda aguardando outro jogador, atualizar tempo na tela
                setTimeout(getMove, 5000);
            }
        }
    });
}
var onMoveEnd = function () {
    boardEl.find('.square-' + squareToHighlight)
        .addClass('highlight-' + otherColor);
};
function endGame() {
    if (game.game_over()) {
        if (game.in_stalemate()) {
            $('#terminoModalBody').html('Empate por afogamento.');
        } else if (game.in_checkmate()) {
            $('#terminoModalBody').html('Checkmate.');
        } else if (game.in_threefold_repetition()) {
            $('#terminoModalBody').html('Empate por repetição.');
        } else if (game.in_draw()) {
            if (gameinsufficient_material()) {
                // sem peças suficientes
                $('#terminoModalBody').html('Empate. Peças Insuficientes.');
            } else {
                $('#terminoModalBody').html('Empate.');
            }
        } else {
            $('#terminoModalBody').html('Fim de Jogo');
        }
        $('#terminoModal').modal('show');
        return true;
    }
    return false;
}
// update the board position after the piece snap
// for castling, en passant, pawn promotion
var onSnapEnd = function () {
    board.position(game.fen());
};
function getImgSrc(piece) {
    return piece_theme.replace('{piece}', game.turn() + piece.toLocaleUpperCase());
}

let init = function () {
    $('[data-toggle=offcanvas]').click(function () {
        $('.row-offcanvas').toggleClass('active');
    });

    var cfg = {
        orientation: white ? 'white' : 'black',
        position: 'start',
        draggable: true,
        onDragStart: onDragStart,
        onDrop: onDrop,
        onMoveEnd: onMoveEnd,
        onSnapEnd: onSnapEnd,
        onMouseoutSquare: onMouseoutSquare,
        onMouseoverSquare: onMouseoverSquare,
        pieceTheme: piece_theme
    };
    board = ChessBoard('board', cfg);
    if (historico.length > 0) {
        game.load_pgn(historico);
        document.querySelector('#PGN').innerHTML = historico.replace(/(\d{1,3}\.)/g, '\n<span style="background-color:yellow">$&</span>');
        //document.querySelector("#PGN").innerHTML = historico;
        board.position(game.fen());
    }
    if (game.turn() === playerColor.charAt(0)) {
        myNameDiv.classList.add(playerTurnDivClass);
    } else {
        otherNameDiv.classList.add(playerTurnDivClass);
        setTimeout(getMove, 10000);
    }
}; // end init()
$(document).ready(init);
